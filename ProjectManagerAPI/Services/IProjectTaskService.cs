namespace ProjectManagerAPI.Services;

using ProjectManagerAPI.DTOs;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskDTO>> GetAllTasksAsync();
    Task<ProjectTaskDTO?> GetTaskByIdAsync(Guid id);
    Task<ProjectTaskDTO> CreateTaskAsync(ProjectTaskDTO taskDto);
    Task<ProjectTaskDTO?> UpdateTaskAsync(Guid id, ProjectTaskDTO taskDto);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<IEnumerable<ProjectTaskDTO>> GetTasksByUserIdAsync(Guid userId);
    Task<bool> MarkTaskAsCompletedAsync(Guid id, Guid userId);
}
