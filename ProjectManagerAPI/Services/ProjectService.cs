namespace ProjectManagerAPI.Services;

using ProjectManagerAPI.Models;
using ProjectManagerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
    {
        return await _dbContext.Projects
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<ProjectDTO?> GetProjectByIdAsync(Guid id)
    {
        var project = await _dbContext.Projects.FindAsync(id).ConfigureAwait(false);
        if (project == null)
            return null;

        return new ProjectDTO
        {
            Id = project.Id,
            Nome = project.Nome,
            Descricao = project.Descricao
        };
    }

    public async Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDto)
    {
        if (projectDto == null)
            throw new ArgumentNullException(nameof(projectDto));

        var project = new Project
        {
            Nome = projectDto.Nome,
            Descricao = projectDto.Descricao
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        projectDto.Id = project.Id;
        return projectDto;
    }

    public async Task<ProjectDTO?> UpdateProjectAsync(Guid id, ProjectDTO projectDto)
    {
        if (projectDto == null)
            throw new ArgumentNullException(nameof(projectDto));

        var project = await _dbContext.Projects.FindAsync(id).ConfigureAwait(false);
        if (project == null)
            return null;

        project.Nome = projectDto.Nome;
        project.Descricao = projectDto.Descricao;

        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return projectDto;
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        var project = await _dbContext.Projects.FindAsync(id).ConfigureAwait(false);
        if (project == null)
            return false;

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
