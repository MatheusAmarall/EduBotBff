using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EduBot.Domain.Entities;

public abstract class Entity {
    [Key]
    public object Id { get; protected set; } = ObjectId.GenerateNewId();
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
}
