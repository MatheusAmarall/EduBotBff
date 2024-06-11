namespace EduBot.Domain.Entities
{
    public class Funcionalidade : Entity
    {
        public string NomeFuncionalidade { get; set; } = string.Empty;
        public bool Disponivel { get; set; } = new();
    }
}
