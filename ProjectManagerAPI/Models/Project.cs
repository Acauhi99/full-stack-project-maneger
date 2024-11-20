namespace ProjectManagerAPI.Models;
public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public ICollection<ProjectTask> Tarefas { get; set; }
}
