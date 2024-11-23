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
