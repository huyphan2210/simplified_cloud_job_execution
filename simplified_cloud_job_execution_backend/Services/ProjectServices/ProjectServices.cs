using simplified_cloud_job_execution_backend.Dtos.Projects;
using simplified_cloud_job_execution_backend.Exceptions;
using simplified_cloud_job_execution_backend.Repositories.ProjectRepository;

namespace simplified_cloud_job_execution_backend.Services.ProjectServices;

public class ProjectServices(IProjectRepository projectRepository, ILogger<ProjectServices> logger) : IProjectServices
{
  private readonly IProjectRepository _projectRepository = projectRepository;
  private readonly ILogger<ProjectServices> _logger = logger;

  public async Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(CancellationToken cancellationToken = default)
  {
    var projects = await _projectRepository.GetAllProjectsAsync(cancellationToken);
    return projects.Select(project => project.ToProjectResponse());
  }

  public async Task<ProjectResponse> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
  {
    var project = await _projectRepository.GetProjectByIdAsync(projectId, cancellationToken)
        ?? throw new NotFoundException("Project not found");

    return project.ToProjectResponse();
  }
}
