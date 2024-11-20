using System.ComponentModel.DataAnnotations;
using ProjectManagerAPI.Models;

namespace ProjectManagerAPI.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "O email deve ser válido.")]
    [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public TipoUsuario TipoUsuario { get; set; }
}

public class RegisterUserDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve ser válido.")]
    [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
    public TipoUsuario TipoUsuario { get; set; }
}

public class LoginUserDTO
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve ser válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Senha { get; set; }
}
