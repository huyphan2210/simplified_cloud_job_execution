using simplified_cloud_job_execution_backend.Dtos.Projects;

namespace simplified_cloud_job_execution_backend.Services.ProjectServices;

public interface IProjectServices
{
  Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(CancellationToken cancellationToken = default);
  Task<ProjectResponse> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}
