using EduBot.Domain.Entities;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EduBot.Application.Interactors.Bot.CreateIntent {
    public class CreateIntentCommandHandler : IRequestHandler<CreateIntentCommand, ErrorOr<CreateIntentCommandResult>> {
        private readonly IUnitOfWork _unitOfWork;
        public CreateIntentCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CreateIntentCommandResult>> Handle(CreateIntentCommand request, CancellationToken cancellationToken) {
            try {
                CreateIntent();

                CreateUtter();

                CreateStory();

                return new CreateIntentCommandResult();
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }

        private void CreateIntent()
        {
            string caminhoIntent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"ChatBotFiles{Path.DirectorySeparatorChar}nlu.yml");

            string yamlConteudo = File.ReadAllText(caminhoIntent);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var data = deserializer.Deserialize<Dictionary<string, object>>(yamlConteudo);

            var nluSection = (List<object>)data["nlu"];
            var intents = new List<IntentRasa>();

            foreach (var item in nluSection) {
                var intentDict = (Dictionary<object, object>)item;

                if (intentDict != null) {
                    string intentName = intentDict["intent"].ToString()!;

                    string examplesText = intentDict["examples"].ToString()!;

                    var examples = examplesText.Split("\n")
                                               .Select(example => example.TrimStart('-', ' ').Trim())
                                               .Where(example => !string.IsNullOrWhiteSpace(example))
                                               .ToList();

                    intents.Add(new IntentRasa { Intent = intentName, Examples = examples });
                }
            }

            var novaIntencao1 = new IntentRasa {
                Intent = "Informacao",
                Examples = new List<string> { "Quero informação" }
            };

            intents.Add(novaIntencao1);

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var novoYamlConteudo = serializer.Serialize(new {
                version = "3.1",
                nlu = intents
            });

            novoYamlConteudo = novoYamlConteudo.Replace("examples:", "examples: |");
            novoYamlConteudo = novoYamlConteudo.Replace("3.1", "\"3.1\"");
            novoYamlConteudo = novoYamlConteudo.Replace("\n  -", "\n    -");

            Console.WriteLine(novoYamlConteudo);

            string novoCaminhoIntent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"ChatBotFiles{Path.DirectorySeparatorChar}nlu_parametrizado.yml");

            File.WriteAllText(novoCaminhoIntent, novoYamlConteudo);
        }

        private void CreateUtter() {
        }

        private void CreateStory() {
            string caminhoStories = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"ChatBotFiles{Path.DirectorySeparatorChar}stories.yml");

            string yamlConteudo = File.ReadAllText(caminhoStories);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var data = deserializer.Deserialize<Dictionary<string, object>>(yamlConteudo);

            var storiesSection = (List<object>)data["stories"];
            var stories = new List<StoryRasa>();

            foreach (var item in storiesSection) {
                var storyDict = (Dictionary<object, object>)item;

                if (storyDict != null) {
                    string storyName = storyDict["story"].ToString()!;

                    var stepsList = (List<object>)storyDict["steps"];
                    var steps = new List<object>();

                    foreach (var stepItem in stepsList) {
                        var stepDict = (Dictionary<object, object>)stepItem;

                        if (stepDict.ContainsKey("intent")) {
                            string intent = stepDict["intent"].ToString()!;
                            steps.Add(new { intent = intent });
                        }
                        else if (stepDict.ContainsKey("action")) {
                            string action = stepDict["action"].ToString()!;
                            steps.Add(new { action = action });
                        }
                    }

                    stories.Add(new StoryRasa { Story = storyName, Steps = steps });
                }
            }

            var novoStory = new StoryRasa {
                Story = "Historinha",
                Steps = new List<object> {
                    new { Intent = "informacao" },
                    new { Action = "utter_informacao" }
                }
            };

            stories.Add(novoStory);

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var novoYamlConteudo = serializer.Serialize(new {
                version = "3.1",
                stories = stories
            });

            novoYamlConteudo = novoYamlConteudo.Replace("'", "");
            novoYamlConteudo = novoYamlConteudo.Replace("3.1", "\"3.1\"");
            novoYamlConteudo = novoYamlConteudo.Replace("\n  -", "\n    -");

            string novoCaminhoStories = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"ChatBotFiles{Path.DirectorySeparatorChar}stories_parametrizado.yml");

            File.WriteAllText(novoCaminhoStories, novoYamlConteudo);
        }
    }
}
