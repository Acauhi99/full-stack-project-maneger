namespace ProjectManagerAPI.Services;

using ProjectManagerAPI.Models;
using ProjectManagerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

public class ProjectTaskService : IProjectTaskService
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectTaskService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectTaskDTO>> GetAllTasksAsync()
    {
        return await _dbContext.ProjectTask
            .Select(t => new ProjectTaskDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Concluida = t.Concluida,
                ProjetoId = t.ProjetoId,
                UsuarioId = t.UsuarioId
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<ProjectTaskDTO?> GetTaskByIdAsync(Guid id)
    {
        var task = await _dbContext.ProjectTask.FindAsync(id).ConfigureAwait(false);

        if (task == null)
            return null;

        return new ProjectTaskDTO
        {
            Id = task.Id,
            Titulo = task.Titulo,
            Descricao = task.Descricao,
            Concluida = task.Concluida,
            ProjetoId = task.ProjetoId,
            UsuarioId = task.UsuarioId
        };
    }

    public async Task<ProjectTaskDTO> CreateTaskAsync(ProjectTaskDTO taskDto)
    {
        if (taskDto == null)
            throw new ArgumentNullException(nameof(taskDto));

        var task = new ProjectTask
        {
            Titulo = taskDto.Titulo,
            Descricao = taskDto.Descricao,
            Concluida = taskDto.Concluida,
            ProjetoId = taskDto.ProjetoId,
            UsuarioId = taskDto.UsuarioId
        };

        _dbContext.ProjectTask.Add(task);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        taskDto.Id = task.Id;
        return taskDto;
    }

    public async Task<ProjectTaskDTO?> UpdateTaskAsync(Guid id, ProjectTaskDTO taskDto)
    {
        if (taskDto == null)
            throw new ArgumentNullException(nameof(taskDto));

        var task = await _dbContext.ProjectTask.FindAsync(id).ConfigureAwait(false);

        if (task == null)
            return null;

        task.Titulo = taskDto.Titulo;
        task.Descricao = taskDto.Descricao;
        task.Concluida = taskDto.Concluida;
        task.ProjetoId = taskDto.ProjetoId;
        task.UsuarioId = taskDto.UsuarioId;

        _dbContext.ProjectTask.Update(task);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return taskDto;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task = await _dbContext.ProjectTask.FindAsync(id).ConfigureAwait(false);

        if (task == null)
            return false;

        _dbContext.ProjectTask.Remove(task);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<IEnumerable<ProjectTaskDTO>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _dbContext.ProjectTask
            .Where(t => t.UsuarioId == userId)
            .Select(t => new ProjectTaskDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Concluida = t.Concluida,
                ProjetoId = t.ProjetoId,
                UsuarioId = t.UsuarioId
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<bool> MarkTaskAsCompletedAsync(Guid id, Guid userId)
    {
        var task = await _dbContext.ProjectTask
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId)
            .ConfigureAwait(false);

        if (task == null)
            return false;

        task.Concluida = true;
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
