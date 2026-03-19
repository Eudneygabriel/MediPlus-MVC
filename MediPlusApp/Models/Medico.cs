namespace MediPlusApp.Models
{
    public class Medico
    {
        public int MedicoId { get; set; }
        public string Nome { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; } // O erro pode estar aqui se faltar este!
        
        // Relacionamento com Especialidade
        public int EspecialidadeId { get; set; }
        public virtual Especialidade Especialidade { get; set; } 
    }
}