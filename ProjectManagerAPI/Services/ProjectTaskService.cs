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

    public async Task<ProjectTaskDTO> CreateTaskAsync(CreateProjectTaskDTO taskDto)
    {
        if (taskDto == null)
            throw new ArgumentNullException(nameof(taskDto));

        var projectExists = await _dbContext.Projects.AnyAsync(p => p.Id == taskDto.ProjetoId).ConfigureAwait(false);

        if (!projectExists)
            throw new InvalidOperationException("Projeto não encontrado.");

        var userExists = await _dbContext.Users.AnyAsync(u => u.Id == taskDto.UsuarioId).ConfigureAwait(false);
        if (!userExists)
            throw new InvalidOperationException("Usuário não encontrado.");

        var task = new ProjectTask
        {
            Titulo = taskDto.Titulo,
            Descricao = taskDto.Descricao,
            Concluida = false,
            ProjetoId = taskDto.ProjetoId,
            UsuarioId = taskDto.UsuarioId
        };

        _dbContext.ProjectTask.Add(task);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

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

    public async Task<ProjectTaskDTO?> UpdateTaskAsync(Guid id, UpdateTaskDTO taskDto)
    {
        if (taskDto == null)
            throw new ArgumentNullException(nameof(taskDto));

        var task = await _dbContext.ProjectTask.FindAsync(id).ConfigureAwait(false);
        if (task == null)
            return null;

        if (taskDto.Titulo != null)
            task.Titulo = taskDto.Titulo;
        if (taskDto.Descricao != null)
            task.Descricao = taskDto.Descricao;
        if (taskDto.Concluida.HasValue)
            task.Concluida = taskDto.Concluida.Value;
        if (taskDto.ProjetoId.HasValue)
        {
            var projectExists = await _dbContext.Projects.AnyAsync(p => p.Id == taskDto.ProjetoId).ConfigureAwait(false);
            if (!projectExists)
                throw new InvalidOperationException("Projeto não encontrado.");
            task.ProjetoId = taskDto.ProjetoId.Value;
        }
        if (taskDto.UsuarioId.HasValue)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == taskDto.UsuarioId).ConfigureAwait(false);
            if (!userExists)
                throw new InvalidOperationException("Usuário não encontrado.");
            task.UsuarioId = taskDto.UsuarioId.Value;
        }

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

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

    public async Task<bool> MarkTaskAsCompletedAsync(Guid taskId, Guid userId)
    {
        var task = await _dbContext.ProjectTask
            .FirstOrDefaultAsync(t => t.Id == taskId)
            .ConfigureAwait(false);

        if (task == null)
            return false;

        if (task.UsuarioId != userId)
            return false;

        task.Concluida = true;
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<bool> MarkTaskAsIncompleteAsync(Guid id, Guid userId)
    {
        try
        {
            var task = await _dbContext.ProjectTask
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId).ConfigureAwait(false);

            if (task == null)
                return false;

            task.Concluida = false;
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
