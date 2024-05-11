namespace EduBot.Application.Common.DTOs {
    public class ParametrizacaoDto {
        public List<Dictionary<string, object>>? Pipeline { get; set; }
        public List<Dictionary<string, object>>? Rules { get; set; }
        public List<Dictionary<string, object>>? Nlu { get; set; }
        public List<Dictionary<string, object>>? Stories { get; set; }
        public List<Dictionary<string, object>>? Policies { get; set; }
        public List<string>? Intents { get; set; }
        public List<string>? Entities { get; set; }
        public Dictionary<string, SlotDto> Slots { get; set; }
        public Dictionary<string, FormDto> Forms { get; set; }
        public Dictionary<string, List<ResponseDto>> Responses { get; set; }
        public List<string>? Actions { get; set; }
        public SessionConfigDto? session_config { get; set; }
    }

    public class SlotDto {
        public string Type { get; set; }
        public bool InfluenceConversation { get; set; }
        public List<MappingDto> Mappings { get; set; }
    }

    public class MappingDto {
        public string Type { get; set; }
        public string Entity { get; set; }
        public List<ConditionDto> Conditions { get; set; }
    }

    public class ConditionDto {
        public string active_loop { get; set; }
    }

    public class FormDto {
        public List<string> required_slots { get; set; }
    }

    public class ResponseDto {
        public string Text { get; set; } = string.Empty;
    }

    public class SessionConfigDto {
        public int Session_expiration_time { get; set; }
        public bool Carry_over_slots_to_new_session { get; set; }
    }
}
