namespace ProjectManagerAPI.Endpoints;

using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;

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

        app.MapGet("/user/tasks", GetUserTasks)
            .RequireAuthorization("Regular");

        app.MapPut("/user/tasks/{id:guid}/complete", CompleteUserTask)
            .RequireAuthorization("Regular");
    }

    private static async Task<IResult> GetUserTasks(IProjectTaskService taskService, HttpContext context)
    {
        var userId = context.User.FindFirst("id")?.Value;
        if (userId == null)
            return Results.Unauthorized();

        var tasks = await taskService.GetTasksByUserIdAsync(Guid.Parse(userId)).ConfigureAwait(false);
        return Results.Ok(tasks);
    }

    private static async Task<IResult> CompleteUserTask(Guid id, IProjectTaskService taskService, HttpContext context)
    {
        var userId = context.User.FindFirst("id")?.Value;
        if (userId == null)
            return Results.Unauthorized();

        var success = await taskService.MarkTaskAsCompletedAsync(id, Guid.Parse(userId)).ConfigureAwait(false);
        return success ? Results.NoContent() : Results.BadRequest("Não foi possível marcar a tarefa como concluída.");
    }

    private static async Task<IResult> GetAllTasks(IProjectTaskService taskService)
    {
        var tasks = await taskService.GetAllTasksAsync().ConfigureAwait(false);
        return Results.Ok(tasks);
    }

    private static async Task<IResult> GetTaskById(Guid id, IProjectTaskService taskService)
    {
        var task = await taskService.GetTaskByIdAsync(id).ConfigureAwait(false);
        return task == null ? Results.NotFound() : Results.Ok(task);
    }

    private static async Task<IResult> CreateTask(ProjectTaskDTO taskDto, IProjectTaskService taskService)
    {
        var createdTask = await taskService.CreateTaskAsync(taskDto).ConfigureAwait(false);
        return Results.Created(new Uri($"/tasks/{createdTask.Id}", UriKind.Relative), createdTask);
    }

    private static async Task<IResult> UpdateTask(Guid id, ProjectTaskDTO taskDto, IProjectTaskService taskService)
    {
        var updatedTask = await taskService.UpdateTaskAsync(id, taskDto).ConfigureAwait(false);
        return updatedTask == null ? Results.NotFound() : Results.Ok(updatedTask);
    }

    private static async Task<IResult> DeleteTask(Guid id, IProjectTaskService taskService)
    {
        var success = await taskService.DeleteTaskAsync(id).ConfigureAwait(false);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
