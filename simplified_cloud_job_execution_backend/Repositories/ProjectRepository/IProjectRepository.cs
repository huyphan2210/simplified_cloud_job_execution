using simplified_cloud_job_execution_backend.Domain.Projects;

namespace simplified_cloud_job_execution_backend.Repositories.ProjectRepository;

public interface IProjectRepository
{
  Task<Project?> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<Project>> GetAllProjectsAsync(CancellationToken cancellationToken = default);
  Task<Project> CreateProjectAsync(Project project, CancellationToken cancellationToken = default);
  Task<Project> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default);
}
