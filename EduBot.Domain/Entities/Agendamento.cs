namespace EduBot.Domain.Entities
{
    public class Agendamento : Entity
    {
        public DateTime DataAgendamento { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
    }
}
