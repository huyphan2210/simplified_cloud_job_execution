namespace simplified_cloud_job_execution_backend.Domain.Projects;

public class Project
{
  public Guid Id { get; set; }
  public required string ProjectName { get; set; }
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public bool IsDeleted { get; set; }
}