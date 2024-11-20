using System.ComponentModel.DataAnnotations;

namespace ProjectManagerAPI.DTOs
{
    public class ProjectTaskDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título da tarefa não pode exceder 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição da tarefa é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição da tarefa não pode exceder 500 caracteres.")]
        public string Descricao { get; set; }

        public bool Concluida { get; set; }

        [Required(ErrorMessage = "O ID do projeto é obrigatório.")]
        public Guid ProjetoId { get; set; }

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public Guid UsuarioId { get; set; }
    }
}
