using System.ComponentModel.DataAnnotations;

namespace MediPlusApp.Models
{
    public class Medico
    {
        [Key]
        public int MedicoId { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        
        // Propriedade para guardar o nome da imagem (ex: "medico1.jpg")
        public string? FotoPath { get; set; } 

        public int EspecialidadeId { get; set; }
        public virtual Especialidade? Especialidade { get; set; }

        // Adicionamos esta lista para o "Include" do Controller funcionar
        // e podermos mostrar as consultas no perfil do médico
        public virtual ICollection<Marcacao> Marcacoes { get; set; } = new List<Marcacao>();
    }
}