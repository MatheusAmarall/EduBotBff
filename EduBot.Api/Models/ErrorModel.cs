using System.Text.Json;

namespace EduBot.Api.Models {
    public class ErrorModel {
        private ErrorModel() { }

        public int Status { get; private init; }
        public string TraceId { get; private init; } = null!;
        public List<string> Errors { get; private init; } = null!;

        public static ErrorModel Create(int status, string traceId, string error) {
            return new ErrorModel {
                TraceId = traceId,
                Status = status,
                Errors = new List<string> { error }
            };
        }

        public static ErrorModel Create(int status, string traceId, List<string> errors) {
            return new ErrorModel {
                TraceId = traceId,
                Status = status,
                Errors = errors
            };
        }

        public static ErrorModel Create(int status, string traceId, IEnumerable<Error> errors) {
            return new ErrorModel {
                TraceId = traceId,
                Status = status,
                Errors = errors.Select(x => x.Description).ToList()
            };
        }

        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
    }
}
