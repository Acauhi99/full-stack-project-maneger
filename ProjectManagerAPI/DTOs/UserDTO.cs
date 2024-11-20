using ProjectManagerAPI.Models;

namespace ProjectManagerAPI.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
}
