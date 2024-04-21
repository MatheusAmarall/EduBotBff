namespace EduBot.Domain.Entities
{
    public class Matricula : Entity
    {
        public string MatriculaUsuario { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = new();
    }
}
