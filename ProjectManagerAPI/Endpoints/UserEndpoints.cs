using Microsoft.AspNetCore.Mvc;
using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;

namespace ProjectManagerAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/api/users/register", async ([FromBody] RegisterUserDTO dto, IUserService userService) =>
            {
                var result = await userService.RegisterAsync(dto).ConfigureAwait(false);
                return result.Match<IResult>(
                    user => Results.Created(new Uri($"/api/users/{user.Id}", UriKind.Relative),
                        new { message = "Usuário registrado com sucesso", user }),
                    error => Results.BadRequest(new { message = error })
                );
            });

            routes.MapPost("/api/users/login", async ([FromBody] LoginUserDTO dto, IUserService userService) =>
            {
                var result = await userService.LoginAsync(dto).ConfigureAwait(false);
                return result.Match<IResult>(
                    token => Results.Ok(new { message = "Login realizado com sucesso", token }),
                    error => Results.UnprocessableEntity(new { message = error })
                );
            });
        }
    }
}
