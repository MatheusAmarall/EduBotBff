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
                    Intent = "Celso gente boa",
                    Examples = new List<string> { "Celso é gente boa?", "Celso é do acre?" }
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
                novoYamlConteudo = novoYamlConteudo.Replace("'", "");

                Console.WriteLine(novoYamlConteudo);

                string novoCaminhoIntent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"ChatBotFiles{Path.DirectorySeparatorChar}nlu_atualizado.yml");

                File.WriteAllText(novoCaminhoIntent, novoYamlConteudo);

                return new CreateIntentCommandResult();
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
