namespace ProjectManagerAPI.DTOs;

public class ProjectTaskDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
    public Guid ProjetoId { get; set; }
    public Guid UsuarioId { get; set; }
}
