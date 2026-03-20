namespace MediPlusApp.Models
{
    public class Paciente
    {
        public int PacienteId { get; set; }

        // Inicializar com string.Empty resolve os avisos CS8618
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telemovel { get; set; } = string.Empty;
        public string SNS { get; set; } = string.Empty;

        // E a lista que adicionámos antes para as marcações
        public virtual ICollection<Marcacao> Marcacoes { get; set; } = new List<Marcacao>();
    }
}