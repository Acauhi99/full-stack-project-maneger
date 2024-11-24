using System.ComponentModel.DataAnnotations;

namespace ProjectManagerAPI.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do projeto não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do projeto é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição do projeto não pode exceder 500 caracteres.")]
        public string Descricao { get; set; }
    }

    public class UpdateProjectDTO
    {
        [StringLength(100, ErrorMessage = "O nome do projeto não pode exceder 100 caracteres.")]
        public string? Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição do projeto não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }
    }

    public class ProjectResponseDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
