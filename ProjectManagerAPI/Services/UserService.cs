using Microsoft.EntityFrameworkCore;
using ProjectManagerAPI.DTOs;
using ProjectManagerAPI.Models;
using ProjectManagerAPI.Utils;

namespace ProjectManagerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public UserService(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<UserDTO?> RegisterAsync(RegisterUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Verificar se o email já está registrado
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email).ConfigureAwait(false))
            {
                return null;
            }

            var hashedPassword = HashPassword(dto.Senha);

            var user = new User
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = hashedPassword,
                TipoUsuario = dto.TipoUsuario,
                Tarefas = new List<ProjectTask>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new UserDTO
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                TipoUsuario = user.TipoUsuario
            };
        }

        public async Task<string?> LoginAsync(LoginUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(dto.Senha, user.Senha))
            {
                return null;
            }

            var token = _jwtHelper.GenerateToken(user);
            return token;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
