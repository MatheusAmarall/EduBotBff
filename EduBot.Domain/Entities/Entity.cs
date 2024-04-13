using System.ComponentModel.DataAnnotations;

namespace EduBot.Domain.Entities;

public abstract class Entity {
    [Key]
    public string Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;

    protected static string GenerateId() {
        return Guid.NewGuid().ToString();
    }
}
