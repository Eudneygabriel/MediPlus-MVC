namespace MediPlusApp.Models;

public class Marcacao {
    public int MarcacaoId { get; set; } // PK
    public DateTime DataHora { get; set; }
    public int MedicoId { get; set; } // FK
    public int PacienteId { get; set; } // FK
    public string Observacoes { get; set; } = "";
}