namespace MediPlusApp.Models;

public class Marcacao {
    public int MarcacaoId { get; set; } // PK
    public DateTime DataHora { get; set; }
    
    public int MedicoId { get; set; } // FK
    // Esta linha abaixo é a "estrada" que liga ao Modelo Medico
    public Medico? Medico { get; set; } 

    public int PacienteId { get; set; } // FK
    // Esta linha abaixo é a "estrada" que liga ao Modelo Paciente
    public Paciente? Paciente { get; set; }

    public string Observacoes { get; set; } = "";

    // NOVO: Estado da consulta (Ex: Confirmada, Realizada, Cancelada)
    // Por defeito, toda a consulta começa como "Confirmada"
    public string Estado { get; set; } = "Confirmada";
}