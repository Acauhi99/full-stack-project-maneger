using ProjectManagerAPI.DTOs;

namespace ProjectManagerAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO?> RegisterAsync(RegisterUserDTO dto);
        Task<string?> LoginAsync(LoginUserDTO dto);
    }
}
