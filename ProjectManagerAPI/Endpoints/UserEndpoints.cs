using Microsoft.AspNetCore.Mvc;
using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;

namespace ProjectManagerAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            // Registro de Usuário
            routes.MapPost("/api/users/register", async ([FromBody] RegisterUserDTO dto, IUserService userService) =>
            {
                var user = await userService.RegisterAsync(dto).ConfigureAwait(false);
                return user != null
                    ? Results.Created(new Uri($"/api/users/{user.Id}", UriKind.Relative), user)
                    : Results.BadRequest("Registro falhou.");
            });

            // Login de Usuário
            routes.MapPost("/api/users/login", async ([FromBody] LoginUserDTO dto, IUserService userService) =>
            {
                var token = await userService.LoginAsync(dto).ConfigureAwait(false);
                return token != null
                    ? Results.Ok(new { Token = token })
                    : Results.Unauthorized();
            });
        }
    }
}
