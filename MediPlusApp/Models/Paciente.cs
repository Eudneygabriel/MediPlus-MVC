namespace MediPlusApp.Models
{
    public class Paciente
    {
        public int PacienteId { get; set; }
        public string Nome { get; set; }
        public string SNS { get; set; } // Necessário para identificação oficial
        public string Email { get; set; } // Para o aviso por email
        public string Telemovel { get; set; } // Para o aviso por SMS/WhatsApp
    }
}