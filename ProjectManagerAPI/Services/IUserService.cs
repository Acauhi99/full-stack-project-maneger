using ProjectManagerAPI.DTOs;
using ProjectManagerAPI.Utils;

namespace ProjectManagerAPI.Services
{
    public interface IUserService
    {
        Task<Result<UserDTO>> RegisterAsync(RegisterUserDTO dto);
        Task<Result<string>> LoginAsync(LoginUserDTO dto);
    }
}
