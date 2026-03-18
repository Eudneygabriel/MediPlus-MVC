namespace MediPlusApp.Models;

public class Medico {
    public int MedicoId { get; set; } // PK
    public string Nome { get; set; } = "";
    public string Cedula { get; set; } = "";
    public string Email { get; set; } = "";
    public int EspecialidadeId { get; set; } // FK
}