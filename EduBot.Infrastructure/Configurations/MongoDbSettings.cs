namespace EduBot.Infrastructure.Configurations {
    public class MongoDbSettings {
        public static string SectionName { get; } = "MongoDbSettings";
        public string? ConnectionString { get; set; } = "";
        public bool UseTransaction { get; set; }
    }
}
