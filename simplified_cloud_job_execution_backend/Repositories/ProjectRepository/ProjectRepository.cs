using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Domain.Projects;
using simplified_cloud_job_execution_backend.Infrastructure.Database;

namespace simplified_cloud_job_execution_backend.Repositories.ProjectRepository;

public class ProjectRepository(AppDbContext dbContext) : IProjectRepository
{
  private readonly AppDbContext _dbContext = dbContext;

  public async Task<Project?> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Projects.FirstOrDefaultAsync(project => project.Id == projectId && project.IsDeleted != true, cancellationToken);
  }

  public async Task<IReadOnlyList<Project>> GetAllProjectsAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Projects
        .Where(project => project.IsDeleted != true)
        .OrderBy(project => project.ProjectName)
        .ToListAsync(cancellationToken);
  }

  public async Task<Project> CreateProjectAsync(Project project, CancellationToken cancellationToken = default)
  {
    _dbContext.Projects.Add(project);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return project;
  }

  public async Task<Project> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default)
  {
    _dbContext.Projects.Update(project);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return project;
  }
}
