using simplified_cloud_job_execution_backend.Domain.Projects;

namespace simplified_cloud_job_execution_backend.Dtos.Projects;

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
