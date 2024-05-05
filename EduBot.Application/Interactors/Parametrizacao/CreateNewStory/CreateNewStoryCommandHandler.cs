using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Services;
using MediatR;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EduBot.Application.Interactors.Bot.CreateNewStory {
    public class CreateNewStoryCommandHandler : IRequestHandler<CreateNewStoryCommand, ErrorOr<Unit>> {
        private readonly IRasaService _rasaService;
        private readonly IMapper _mapper;
        public CreateNewStoryCommandHandler(IRasaService rasaService, IMapper mapper) {
            _rasaService = rasaService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Unit>> Handle(CreateNewStoryCommand request, CancellationToken cancellationToken) {
            try {
                var loadedDomain = await _rasaService.RetrieveLoadedDomainAsync("thisismysecret");

                if (loadedDomain == null) {
                    return Error.Validation(description: "Erro ao consultar dados do EduBot");
                }

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();


                List<Dictionary<string, object>> pipeline = RetornaPipeline();

                List<Dictionary<string, object>> policies = RetornaPolicies();

                List<Dictionary<string, object>> nlu = RetornaNlu();

                List<Dictionary<string, object>> rules = RetornaRules();

                List<Dictionary<string, object>> stories = RetornaStories();

                foreach (var keyValuePair in loadedDomain.Responses) {
                    var responseList = keyValuePair.Value;

                    foreach (var response in responseList) {
                        if (!string.IsNullOrEmpty(response.Text)) {
                            response.Text = response.Text.Replace("\r\n", "\\n").Replace("\n", "\\n");
                            response.Text = $"\"{response.Text}\"";
                        }
                    }
                }

                loadedDomain.Pipeline = pipeline;
                loadedDomain.Policies = policies;
                loadedDomain.Rules = rules;
                loadedDomain.Nlu = nlu;
                loadedDomain.Stories = stories;

                AdicionaNovaParametrizacao(loadedDomain, request);

                var yamlTreinamento = serializer.Serialize(loadedDomain);

                yamlTreinamento = yamlTreinamento.Replace("examples:", "examples: |");
                yamlTreinamento = yamlTreinamento.Replace("'", "");
                yamlTreinamento = yamlTreinamento.Replace("e2eActions", "e2e_actions");
                yamlTreinamento = yamlTreinamento.Replace("influenceConversation", "influence_conversation");
                yamlTreinamento = yamlTreinamento.Replace("requiredSlots", "required_slots");
                yamlTreinamento = yamlTreinamento.Replace("activeLoop", "active_loop");
                yamlTreinamento = yamlTreinamento.Replace("sessionConfig", "session_config");
                yamlTreinamento = yamlTreinamento.Replace("requested_slot", "- requested_slot");
                yamlTreinamento = yamlTreinamento.Replace("\n  -", "\n    -");
                yamlTreinamento = yamlTreinamento.Replace("3.1", "\"3.1\"");

                var trainedModel = await _rasaService.TrainRasaModelAsync("thisismysecret", yamlTreinamento);

                if (trainedModel.IsSuccessStatusCode) {
                    if (trainedModel.Headers.TryGetValues("filename", out var values)) {
                        string filename = values.FirstOrDefault();

                        await _rasaService.ReplaceLoadedModelAsync("thisismysecret", new ReplaceLoadedModelDto($"models{Path.DirectorySeparatorChar}{filename}"));
                    }
                }
                else {
                    return Error.Validation(description: "Erro ao parametrizar o EduBot");
                }

                return Unit.Value;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }

        private void AdicionaNovaParametrizacao(RetrieveLoadedDomainDto loadedDomain, CreateNewStoryCommand request) {
            List<Response> respostasUtter = _mapper.Map<List<Response>>(request.Respostas);

            loadedDomain.Intents!.Add(request.TituloPergunta);
            loadedDomain.Nlu!.Add(
                new Dictionary<string, object>
                {
                        { "intent", request.TituloPergunta },
                        { "examples", request.Perguntas }
                }
            );
            loadedDomain.Responses.Add($"utter_{request.TituloPergunta}", respostasUtter);
            loadedDomain.Stories!.Add(
                new Dictionary<string, object>
                {
                        { "story", $"story_{request.TituloPergunta}" },
                        { "steps", new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    { "intent", request.TituloPergunta }
                                },
                                new Dictionary<string, object>
                                {
                                    { "action", $"utter_{request.TituloPergunta}" }
                                }
                            }
                        }
                }
            );
        }

        private List<Dictionary<string, object>> RetornaPipeline() {
            var pipeline = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> { { "name", "WhitespaceTokenizer" } },
                new Dictionary<string, object> { { "name", "RegexFeaturizer" } },
                new Dictionary<string, object> { { "name", "LexicalSyntacticFeaturizer" } },
                new Dictionary<string, object> { { "name", "CountVectorsFeaturizer" } },
                new Dictionary<string, object>
                {
                    { "name", "CountVectorsFeaturizer" },
                    { "analyzer", "char_wb" },
                    { "min_ngram", 1 },
                    { "max_ngram", 4 }
                },
                new Dictionary<string, object>
                {
                    { "name", "DIETClassifier" },
                    { "epochs", 100 },
                    { "constrain_similarities", true }
                },
                new Dictionary<string, object> { { "name", "EntitySynonymMapper" } },
                new Dictionary<string, object>
                {
                    { "name", "ResponseSelector" },
                    { "epochs", 100 },
                    { "constrain_similarities", true }
                },
                new Dictionary<string, object>
                {
                    { "name", "FallbackClassifier" },
                    { "threshold", 0.3 },
                    { "ambiguity_threshold", 0.1 }
                }
            };

            return pipeline;
        }

        private List<Dictionary<string, object>> RetornaStories() 
        {
            var stories = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "story", "path 1" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "cumprimento" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_cumprimento" }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "afirmacao" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_afirmacao" }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "bot_challenge" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_bot_challenge" }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "despedida" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_goodbye" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "path 2" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "lista_espera" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_lista_espera" }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "matricula" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_matricula" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "escolher lista de materiais" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "material" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_material_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_material_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_material" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "action_mostrar_lista_material" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "escolher cardapio" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "cardapio" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_cardapio_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_cardapio_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_cardapio" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "action_mostrar_cardapio" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "visitante escolha" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "visitante" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_visitante_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_visitante_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_visitante" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "action_mostrar_escolha_visitante" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "pedir cronograma de aulas path 4" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "aulas" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_cronograma_aulas" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "quando faz elogio ao edubot path 5" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "otimo" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_otimo" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "quando faz negacao ao edubot path 6" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "negacao" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "utter_negacao" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "interactive_story_1" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "material" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_material_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_material_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_material" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "material_escolha" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "interactive_story_2" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "cardapio" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_cardapio_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_cardapio_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_cardapio" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "cardapio_escolha" }
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "story", "interactive_story_3" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "intent", "visitante" }
                            },
                            new Dictionary<string, object>
                            {
                                { "action", "tipo_visitante_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "tipo_visitante_form" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "tipo_visitante" }
                                    }
                                }
                            },
                            new Dictionary<string, object>
                            {
                                { "intent", "visitante_escolha" }
                            }
                        }
                    }
                }
            };

            return stories;
        }

        private List<Dictionary<string, object>> RetornaNlu() {
            var nlu = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "intent", "cumprimento" },
                    { "examples", new List<string>
                        {
                            "o",
                            "oi",
                            "oii",
                            "ola",
                            "bom dia",
                            "boa tarde",
                            "boa noite",
                            "Olá, como vai?",
                            "E aí?",
                            "Tudo bem?",
                            "Como vai?"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "despedida" },
                    { "examples", new List<string>
                        {
                            "tchau",
                            "adeus",
                            "até mais",
                            "tiau",
                            "Até mais",
                            "Até logo",
                            "Até breve"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "afirmacao" },
                    { "examples", new List<string>
                        {
                            "sim",
                            "s",
                            "claro",
                            "sem dúvidas",
                            "com certeza",
                            "obrigado",
                            "ok",
                            "OK",
                            "O.K",
                            "O.K.",
                            "okay"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "negacao" },
                    { "examples", new List<string>
                        {
                            "não",
                            "n",
                            "Não, obrigado",
                            "Não quero",
                            "Não, não estou interessado"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "otimo" },
                    { "examples", new List<string>
                        {
                            "perfeito",
                            "ótimo",
                            "incrivel",
                            "maravilhoso",
                            "excelente",
                            "muito obrigado"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "bot_challenge" },
                    { "examples", new List<string>
                        {
                            "você é um robô?",
                            "você é um ser humano?",
                            "estou conversando com um robô?",
                            "estou conversando com um ser humano?",
                            "Posso saber se estou conversando com uma IA?",
                            "Estou interagindo com um programa de computador?",
                            "Você é um chatbot ou uma pessoa real?"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "material" },
                    { "examples", new List<string>
                        {
                            "preciso da lista de material",
                            "lista de material",
                            "qual material o aluno precisa ?",
                            "Gostaria de receber a lista de material escolar.",
                            "Poderia me fornecer a lista de materiais necessários para o aluno?",
                            "Qual é a lista de material escolar que o aluno precisa?",
                            "Pode me ajudar com a lista de material escolar?",
                            "Estou procurando pela lista de material escolar."
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "cardapio" },
                    { "examples", new List<string>
                        {
                            "preciso dom cardapio",
                            "lanche",
                            "merenda",
                            "gostaria de verificar o lanche",
                            "oque os alunos comem",
                            "oque tem de comida",
                            "refeicões",
                            "qual é a merenda?",
                            "oque tem de merenda>",
                            "oque tem de lanche?",
                            "oque é servido para alimentação?",
                            "oque é servido de merenda?",
                            "oque é servido de lanche?",
                            "cardapio maternal",
                            "cardapio pre-escola",
                            "cardapio pre escola"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "cardapio_escolha" },
                    { "examples", new List<string>
                        {
                            "[cardápio maternal](tipo_cardapio)",
                            "[cardápio pré-escola](tipo_cardapio)"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "material_escolha" },
                    { "examples", new List<string>
                        {
                            "[maternal](tipo_material)",
                            "[pre-escola](tipo_material)"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "visitante_escolha" },
                    { "examples", new List<string>
                        {
                            "[Cardápio](tipo_visitante)",
                            "[Matricula](tipo_visitante)",
                            "[Lista de Espera](tipo_visitante)",
                            "[Lista de Materiais](tipo_visitante)"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "lista_espera" },
                    { "examples", new List<string>
                        {
                            "lista de espera",
                            "gostaria de saber onde encontro a lista de espera",
                            "Onde encontro a lista de espera?",
                            "Como posso acessar a lista de espera?",
                            "Gostaria de informações sobre a lista de espera",
                            "Estou interessado na lista de espera"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "matricula" },
                    { "examples", new List<string>
                        {
                            "matricula",
                            "gostaria de saber como faço a matricula",
                            "Como faço para realizar a matrícula?",
                            "Gostaria de informações sobre o processo de matrícula.",
                            "Como posso me matricular?",
                            "Quero fazer a matrícula. Como procedo?",
                            "Estou interessado em me matricular. Qual é o procedimento?"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "aulas" },
                    { "examples", new List<string>
                        {
                            "cronograma de aulas",
                            "Qual é o cronograma das aulas?",
                            "O cronograma de aulas está disponível?",
                            "Como posso acessar o cronograma de aulas?",
                            "Onde posso encontrar o cronograma de aulas?"
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "intent", "visitante" },
                    { "examples", new List<string>
                        {
                            "visitante"
                        }
                    }
                }
            };

            return nlu;
        }

        private List<Dictionary<string, object>> RetornaPolicies() {
            var policies = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> { { "name", "MemoizationPolicy" } },
                new Dictionary<string, object> { { "name", "RulePolicy" } },
                new Dictionary<string, object>
                {
                    { "name", "UnexpecTEDIntentPolicy" },
                    { "max_history", 5 },
                    { "epochs", 100 }
                },
                new Dictionary<string, object>
                {
                    { "name", "TEDPolicy" },
                    { "max_history", 5 },
                    { "epochs", 100 },
                    { "constrain_similarities", true }
                }
            };

            return policies;
        }

        private List<Dictionary<string, object>>  RetornaRules() {
            var rules = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "rule", "ativar_o_form_solicitar_material" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "intent", "material" } },
                            new Dictionary<string, object> { { "action", "tipo_material_form" } },
                            new Dictionary<string, object> { { "active_loop", "tipo_material_form" } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "rule", "enviar_o_form_solicitar_material" },
                    { "condition", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "active_loop", "tipo_material_form" } }
                        }
                    },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "action", "tipo_material_form" } },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object> { { "action", "action_mostrar_lista_material" } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "rule", "ativar_o_form_solicitar_cardapio" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "intent", "cardapio" } },
                            new Dictionary<string, object> { { "action", "tipo_cardapio_form" } },
                            new Dictionary<string, object> { { "active_loop", "tipo_cardapio_form" } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "rule", "enviar_o_form_solicitar_cardapio" },
                    { "condition", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "active_loop", "tipo_cardapio_form" } }
                        }
                    },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "action", "tipo_cardapio_form" } },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object> { { "action", "action_mostrar_cardapio" } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "rule", "ativar_o_form_visitante_options" },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "intent", "visitante" } },
                            new Dictionary<string, object> { { "action", "tipo_visitante_form" } },
                            new Dictionary<string, object> { { "active_loop", "tipo_visitante_form" } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "rule", "enviar_o_form_visitante_options" },
                    { "condition", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "active_loop", "tipo_visitante_form" } }
                        }
                    },
                    { "steps", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object> { { "action", "tipo_visitante_form" } },
                            new Dictionary<string, object>
                            {
                                { "active_loop", "null" }
                            },
                            new Dictionary<string, object>
                            {
                                { "slot_was_set", new Dictionary<string, string>
                                    {
                                        { "requested_slot", "null" }
                                    }
                                }
                            },
                            new Dictionary<string, object> { { "action", "action_mostrar_escolha_visitante" } }
                        }
                    }
                }
            };

            return rules;
        }
    }
}
