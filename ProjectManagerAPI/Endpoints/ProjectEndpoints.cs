namespace ProjectManagerAPI.Endpoints;

using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/projects", GetAllProjects)
            .RequireAuthorization();

        app.MapGet("/projects/{id:guid}", GetProjectById)
            .RequireAuthorization();

        app.MapPost("/projects", CreateProject)
            .RequireAuthorization("Admin");

        app.MapPut("/projects/{id:guid}", UpdateProject)
            .RequireAuthorization("Admin");

        app.MapDelete("/projects/{id:guid}", DeleteProject)
            .RequireAuthorization("Admin");

        app.MapGet("/projects/reports", GetTasksPerProject)
            .RequireAuthorization("Admin");
    }

    private static async Task<IResult> CreateProject(ProjectDTO projectDto, IProjectService projectService)
    {
        try
        {
            var createdProject = await projectService.CreateProjectAsync(projectDto).ConfigureAwait(false);
            var uri = new Uri($"/projects/{createdProject.Id}", UriKind.Relative);
            return Results.Created(uri, createdProject);
        }
        catch (ArgumentNullException)
        {
            return Results.BadRequest(new { message = "Dados do projeto são inválidos." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao salvar o projeto no banco de dados." });
        }
    }

    private static async Task<IResult> UpdateProject(Guid id, UpdateProjectDTO projectDto, IProjectService projectService)
    {
        try
        {
            var updatedProject = await projectService.UpdateProjectAsync(id, projectDto).ConfigureAwait(false);
            return updatedProject == null
                ? Results.NotFound(new { message = "Projeto não encontrado." })
                : Results.Ok(updatedProject);
        }
        catch (ArgumentNullException)
        {
            return Results.BadRequest(new { message = "Dados do projeto são inválidos." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao atualizar o projeto no banco de dados." });
        }
    }

    private static async Task<IResult> DeleteProject(Guid id, IProjectService projectService)
    {
        try
        {
            var success = await projectService.DeleteProjectAsync(id).ConfigureAwait(false);
            return success
                ? Results.NoContent()
                : Results.NotFound(new { message = "Projeto não encontrado." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao excluir o projeto no banco de dados." });
        }
    }

    private static async Task<IResult> GetProjectById(Guid id, IProjectService projectService)
    {
        try
        {
            var project = await projectService.GetProjectByIdAsync(id).ConfigureAwait(false);
            return project == null
                ? Results.NotFound(new { message = "Projeto não encontrado." })
                : Results.Ok(project);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
    }

    private static async Task<IResult> GetAllProjects(IProjectService projectService)
    {
        try
        {
            var projects = await projectService.GetAllProjectsAsync().ConfigureAwait(false);
            return Results.Ok(projects);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
    }

    private static async Task<IResult> GetTasksPerProject(IProjectService projectService)
    {
        try
        {
            var report = await projectService.GetTasksPerProjectAsync().ConfigureAwait(false);
            return Results.Ok(report);
        }
        catch (DbUpdateException)
        {
            return Results.BadRequest(new { message = "Erro ao acessar o banco de dados." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }
}
