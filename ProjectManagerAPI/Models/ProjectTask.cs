namespace ProjectManagerAPI.Models;
public class ProjectTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
    public Guid ProjetoId { get; set; }
    public Guid UsuarioId { get; set; }
}
