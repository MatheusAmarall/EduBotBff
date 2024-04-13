using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EduBot.Infrastructure.Identity {
    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<Guid> {
    }
}