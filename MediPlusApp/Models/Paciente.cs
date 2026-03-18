namespace MediPlusApp.Models;

public class Paciente {
    public int PacienteId { get; set; } // PK
    public string Nome { get; set; } = "";
    public string SNS { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DataNascimento { get; set; }
    public string Telemovel { get; set; } = "";
}