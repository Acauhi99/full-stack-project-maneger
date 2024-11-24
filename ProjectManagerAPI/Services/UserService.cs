using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
                return Result.Failure<UserDTO>("Email e senha são obrigatórios");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email).ConfigureAwait(false))
                return Result.Failure<UserDTO>("Email já cadastrado no sistema");

            var hashedPassword = HashPassword(dto.Senha);

            var user = new User
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                Senha = hashedPassword,
                TipoUsuario = dto.TipoUsuario,
                Tarefas = new List<ProjectTask>()
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Result.Success(new UserDTO
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email,
                    TipoUsuario = user.TipoUsuario
                });
            }
            catch (DbUpdateException dbEx)
            {
                return Result.Failure<UserDTO>($"Erro ao salvar usuário no banco de dados: {dbEx.Message}");
            }
            catch (InvalidOperationException ioEx)
            {
                return Result.Failure<UserDTO>($"Erro de operação inválida: {ioEx.Message}");
            }
        }

        public async Task<Result<string>> LoginAsync(LoginUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
                return Result.Failure<string>("Email e senha são obrigatórios");

            try
            {
                var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email.Trim())
                .ConfigureAwait(false);

                if (user == null)
                    return Result.Failure<string>("Email ou senha incorretos");

                if (!VerifyPassword(dto.Senha, user.Senha))
                    return Result.Failure<string>("Email ou senha incorretos");

                var token = _jwtHelper.GenerateToken(user);

                return Result.Success(token);
            }
            catch (DbUpdateException dbEx)
            {
                return Result.Failure<string>($"Erro ao acessar banco de dados: {dbEx.Message}");
            }
            catch (InvalidOperationException ioEx)
            {
                return Result.Failure<string>($"Erro de operação inválida: {ioEx.Message}");
            }
            catch (SecurityTokenException stEx)
            {
                return Result.Failure<string>($"Erro ao gerar token: {stEx.Message}");
            }
        }

        private static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentException("Hashed password cannot be null or empty", nameof(hashedPassword));

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
