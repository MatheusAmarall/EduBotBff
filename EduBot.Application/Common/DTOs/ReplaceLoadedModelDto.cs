using System.Text.Json.Serialization;

namespace EduBot.Application.Common.DTOs {
    public class ReplaceLoadedModelDto {
        public ReplaceLoadedModelDto(string modelFile) {
            ModelFile = modelFile;
        }

        [JsonPropertyName("model_file")]
        public string ModelFile { get; set; } = string.Empty;

    }
}
