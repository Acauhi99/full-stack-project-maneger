namespace ProjectManagerAPI.Models;
using System.Text.Json.Serialization;
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
    public ICollection<ProjectTask> Tarefas { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoUsuario
{
    Admin = 0,
    Regular = 1
}
