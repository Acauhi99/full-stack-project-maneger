namespace ProjectManagerAPI.Services;

using ProjectManagerAPI.DTOs;

public interface IProjectService
{
    Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
    Task<ProjectDTO?> GetProjectByIdAsync(Guid id);
    Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDto);
    Task<ProjectResponseDTO?> UpdateProjectAsync(Guid id, UpdateProjectDTO projectDto);
    Task<bool> DeleteProjectAsync(Guid id);
    Task<IEnumerable<ProjectReportDTO>> GetTasksPerProjectAsync();
}
