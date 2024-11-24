namespace ProjectManagerAPI.Endpoints;

using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public static class ProjectTaskEndpoints
{
    public static void MapProjectTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/tasks", GetAllTasks)
            .RequireAuthorization("Admin");

        app.MapGet("/tasks/{id:guid}", GetTaskById)
            .RequireAuthorization("Admin");

        app.MapPost("/tasks", CreateTask)
            .RequireAuthorization("Admin");

        app.MapPut("/tasks/{id:guid}", UpdateTask)
            .RequireAuthorization("Admin");

        app.MapDelete("/tasks/{id:guid}", DeleteTask)
            .RequireAuthorization("Admin");

        app.MapGet("/tasks/user", GetUserTasks)
            .RequireAuthorization();

        app.MapPut("/tasks/user/complete", CompleteUserTask)
            .RequireAuthorization("Regular");
    }

    private static async Task<IResult> GetAllTasks(IProjectTaskService taskService)
    {
        try
        {
            var tasks = await taskService.GetAllTasksAsync().ConfigureAwait(false);
            return Results.Ok(tasks);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
    }

    private static async Task<IResult> GetTaskById(Guid id, IProjectTaskService taskService)
    {
        try
        {
            var task = await taskService.GetTaskByIdAsync(id).ConfigureAwait(false);
            return task == null
                ? Results.NotFound(new { message = "Tarefa não encontrada." })
                : Results.Ok(task);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
    }

    private static async Task<IResult> CreateTask(CreateProjectTaskDTO taskDto, IProjectTaskService taskService)
    {
        try
        {
            var createdTask = await taskService.CreateTaskAsync(taskDto).ConfigureAwait(false);
            var uri = new Uri($"/tasks/{createdTask.Id}", UriKind.Relative);
            return Results.Created(uri, createdTask);
        }
        catch (ArgumentNullException)
        {
            return Results.BadRequest(new { message = "Dados da tarefa são inválidos." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao salvar a tarefa no banco de dados." });
        }
    }

    private static async Task<IResult> UpdateTask(Guid id, UpdateTaskDTO taskDto, IProjectTaskService taskService)
    {
        try
        {
            var updatedTask = await taskService.UpdateTaskAsync(id, taskDto).ConfigureAwait(false);
            return updatedTask == null
                ? Results.NotFound(new { message = "Tarefa não encontrada." })
                : Results.Ok(updatedTask);
        }
        catch (ArgumentNullException)
        {
            return Results.BadRequest(new { message = "Dados da tarefa são inválidos." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao atualizar a tarefa no banco de dados." });
        }
    }

    private static async Task<IResult> DeleteTask(Guid id, IProjectTaskService taskService)
    {
        try
        {
            var success = await taskService.DeleteTaskAsync(id).ConfigureAwait(false);
            return success
                ? Results.NoContent()
                : Results.NotFound(new { message = "Tarefa não encontrada." });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao excluir a tarefa no banco de dados." });
        }
    }

    private static async Task<IResult> GetUserTasks(IProjectTaskService taskService, HttpContext context)
    {
        try
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Results.Unauthorized();

            var tasks = await taskService.GetTasksByUserIdAsync(Guid.Parse(userId)).ConfigureAwait(false);
            return Results.Ok(tasks);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { message = "ID do usuário inválido." });
        }
    }

    private static async Task<IResult> CompleteUserTask(CompleteTaskDTO request, IProjectTaskService taskService, HttpContext context)
    {
        try
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Results.Unauthorized();

            var success = await taskService.MarkTaskAsCompletedAsync(request.TaskId, Guid.Parse(userId))
                .ConfigureAwait(false);

            return success
                ? Results.NoContent()
                : Results.BadRequest(new { message = "Não foi possível marcar a tarefa como concluída. Verifique se a tarefa existe e pertence a você." });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao atualizar a tarefa no banco de dados." });
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { message = "ID do usuário inválido." });
        }
    }
}
