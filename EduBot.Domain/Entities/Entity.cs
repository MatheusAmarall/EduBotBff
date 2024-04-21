using System.ComponentModel.DataAnnotations;

namespace EduBot.Domain.Entities;

public abstract class Entity {
    [Key]
    public object Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
}
