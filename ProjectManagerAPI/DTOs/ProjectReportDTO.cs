using System.ComponentModel.DataAnnotations;

namespace ProjectManagerAPI.DTOs
{
    public class ProjectReportDTO
    {
        [Required(ErrorMessage = "O ID do projeto é obrigatório.")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do projeto não pode exceder 100 caracteres.")]
        public string ProjectName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "A contagem de tarefas deve ser um número não negativo.")]
        public int TaskCount { get; set; }
    }
}
