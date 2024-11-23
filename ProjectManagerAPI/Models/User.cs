namespace ProjectManagerAPI.Models;
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
    public ICollection<ProjectTask> Tarefas { get; set; }
}

public enum TipoUsuario
{
    Admin = 0,
    Regular = 1
}
