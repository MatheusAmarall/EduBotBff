using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Services;
using EduBot.Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EduBot.Application.Interactors.Parametrizacao.CreateNewStory {
    public class CreateNewStoryCommandHandler : IRequestHandler<CreateNewStoryCommand, ErrorOr<Unit>> {
        private readonly IRasaService _rasaService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateNewStoryCommandHandler(IRasaService rasaService, IMapper mapper, IUnitOfWork unitOfWork) {
            _rasaService = rasaService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(CreateNewStoryCommand request, CancellationToken cancellationToken) {
            try {
                var parametrizacoes = await _unitOfWork.Parametrizacoes.GetAllAsync(cancellationToken);

                if (parametrizacoes.Count == 0) {
                    return Error.Validation(description: "Erro ao parametrizar, contate o suporte!");
                }

                await CriaNovaFuncionalidade(request.TituloPergunta);

                var parametrizacao = parametrizacoes.First();
                var parametrizacaoDto = _mapper.Map<ParametrizacaoDto>(parametrizacao);

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                AdicionaNovaParametrizacao(parametrizacaoDto, request);

                foreach (var keyValuePair in parametrizacaoDto.Responses) {
                    var responseList = keyValuePair.Value;

                    foreach (var response in responseList) {
                        if (!string.IsNullOrEmpty(response.Text)) {
                            response.Text = response.Text.Replace("\r\n", "\\n").Replace("\n", "\\n");
                            response.Text = $"\"{response.Text}\"";
                        }
                    }
                }

                await AtualizaBanco(parametrizacaoDto, parametrizacao);

                var yamlTreinamento = serializer.Serialize(parametrizacaoDto);

                yamlTreinamento = yamlTreinamento.Replace("examples:", "examples: |");
                yamlTreinamento = yamlTreinamento.Replace("'", "");
                yamlTreinamento = yamlTreinamento.Replace("e2eActions", "e2e_actions");
                yamlTreinamento = yamlTreinamento.Replace("influenceConversation", "influence_conversation");
                yamlTreinamento = yamlTreinamento.Replace("requiredSlots", "required_slots");
                yamlTreinamento = yamlTreinamento.Replace("activeLoop", "active_loop");
                yamlTreinamento = yamlTreinamento.Replace("sessionConfig", "session_config");
                yamlTreinamento = yamlTreinamento.Replace("requested_slot", "- requested_slot");
                yamlTreinamento = yamlTreinamento.Replace("\n  -", "\n    -");
                // Remover aspas duplas e triplas
                yamlTreinamento = Regex.Replace(yamlTreinamento, "\"{3}([^\"]*)\"{3}", "\"$1\"");
                yamlTreinamento = Regex.Replace(yamlTreinamento, "\"{2}([^\"]*)\"{2}", "\"$1\"");

                var trainedModel = await _rasaService.TrainRasaModelAsync("thisismysecret", yamlTreinamento);

                if (trainedModel.IsSuccessStatusCode) {
                    if (trainedModel.Headers.TryGetValues("filename", out var values)) {
                        string filename = values.FirstOrDefault()!;

                        await _rasaService.ReplaceLoadedModelAsync("thisismysecret", new ReplaceLoadedModelDto($"models{Path.DirectorySeparatorChar}{filename}"));

                        await DisponibilizaNovaFuncionalidade(request.TituloPergunta);
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

        private void AdicionaNovaParametrizacao(ParametrizacaoDto loadedDomain, CreateNewStoryCommand request) {
            List<ResponseDto> respostasUtter = _mapper.Map<List<ResponseDto>>(request.Respostas);

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

        private async Task AtualizaBanco(ParametrizacaoDto parametrizacaoDto, Domain.Entities.Parametrizacao parametrizacao) {
            var novaParametrizacao = _mapper.Map<Domain.Entities.Parametrizacao>(parametrizacaoDto);

            parametrizacao.Nlu = novaParametrizacao.Nlu;
            parametrizacao.Intents = novaParametrizacao.Intents;
            parametrizacao.Stories = novaParametrizacao.Stories;
            parametrizacao.Responses = novaParametrizacao.Responses;
            _unitOfWork.Parametrizacoes.Update(parametrizacao, new CancellationToken());

            await _unitOfWork.SaveChangesAsync(new CancellationToken());
        }

        private async Task CriaNovaFuncionalidade(string nomeFuncionalidade) {
            var funcionalidade = await _unitOfWork.Funcionalidades.GetFuncionalidadeByNome(nomeFuncionalidade);

            if (funcionalidade is null) {
                var novaFuncionalidade = new Funcionalidade() {
                    NomeFuncionalidade = nomeFuncionalidade,
                    Disponivel = false
                };

                _unitOfWork.Funcionalidades.Add(novaFuncionalidade, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(new CancellationToken());
            }
            else {
                throw new InvalidOperationException("Funcionalidade já existente");
            }
        }

        private async Task DisponibilizaNovaFuncionalidade(string nomeFuncionalidade) {
            var funcionalidade = await _unitOfWork.Funcionalidades.GetFuncionalidadeByNome(nomeFuncionalidade);

            if (funcionalidade is not null) {
                funcionalidade.Disponivel = true;

                _unitOfWork.Funcionalidades.Update(funcionalidade, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(new CancellationToken());
            }
        }
    }
}
