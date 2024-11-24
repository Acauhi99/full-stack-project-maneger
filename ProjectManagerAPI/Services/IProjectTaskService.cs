namespace ProjectManagerAPI.Services;

using ProjectManagerAPI.DTOs;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskDTO>> GetAllTasksAsync();
    Task<ProjectTaskDTO?> GetTaskByIdAsync(Guid id);
    Task<ProjectTaskDTO> CreateTaskAsync(CreateProjectTaskDTO taskDto);
    Task<ProjectTaskDTO?> UpdateTaskAsync(Guid id, UpdateTaskDTO taskDto);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<IEnumerable<ProjectTaskDTO>> GetTasksByUserIdAsync(Guid userId);
    Task<bool> MarkTaskAsCompletedAsync(Guid id, Guid userId);
}
