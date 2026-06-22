using simplified_cloud_job_execution_backend.Dtos.Projects;
using simplified_cloud_job_execution_backend.Domain.Projects;

namespace simplified_cloud_job_execution_backend.Services.ProjectServices;

public static class ProjectServiceExtensions
{
  public static ProjectResponse ToProjectResponse(this Project project)
  {
    return new ProjectResponse
    {
      Id = project.Id,
      ProjectName = project.ProjectName,
      Description = project.Description,
      CreatedAt = project.CreatedAt,
      UpdatedAt = project.UpdatedAt
    };
  }
}
