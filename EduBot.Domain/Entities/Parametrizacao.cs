namespace EduBot.Domain.Entities
{
    public class Parametrizacao : Entity
    {
        public List<Dictionary<string, object>>? Pipeline { get; set; }
        public List<Dictionary<string, object>>? Rules { get; set; }
        public List<Dictionary<string, object>>? Nlu { get; set; }
        public List<Dictionary<string, object>>? Stories { get; set; }
        public List<Dictionary<string, object>>? Policies { get; set; }
        public List<string>? Intents { get; set; }
        public List<string>? Entities { get; set; }
        public Dictionary<string, Slot> Slots { get; set; }
        public Dictionary<string, Form> Forms { get; set; }
        public Dictionary<string, List<Response>> Responses { get; set; }
        public List<string>? Actions { get; set; }
        public SessionConfig? session_config { get; set; }
    }

    public class Slot {
        public string Type { get; set; }
        public bool InfluenceConversation { get; set; }
        public List<Mapping> Mappings { get; set; }
    }

    public class Mapping {
        public string Type { get; set; }
        public string Entity { get; set; }
        public List<Condition> Conditions { get; set; }
    }

    public class Condition {
        public string active_loop { get; set; }
    }

    public class Form {
        public List<string> required_slots { get; set; }
    }

    public class Response {
        public string Text { get; set; } = string.Empty;
    }

    public class SessionConfig {
        public int Session_expiration_time { get; set; }
        public bool Carry_over_slots_to_new_session { get; set; }
    }
}
