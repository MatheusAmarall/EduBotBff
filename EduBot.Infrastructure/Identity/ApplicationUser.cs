using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EduBot.Infrastructure.Identity {
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid> {
        public string Matricula { get; set; } = string.Empty;
    }
}