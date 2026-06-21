using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Infrastructure.Database;

namespace simplified_cloud_job_execution_backend.Repositories.JobRepository;

public class JobRepository(AppDbContext dbContext) : IJobRepository
{
  private readonly AppDbContext _dbContext = dbContext;

  public async Task<Job> CreateJobAsync(Job job, CancellationToken cancellationToken = default)
  {
    _dbContext.Jobs.Add(job);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return job;
  }

  public async Task<Job?> GetJobByIdAsync(Guid jobId, CancellationToken cancellationToken = default)
  {
    var job = await _dbContext.Jobs.FirstOrDefaultAsync(
      job => job.Id == jobId && job.IsDeleted != true,
      cancellationToken
    );

    return job;
  }

  public async Task<Job> UpdateJobAsync(Job job, CancellationToken cancellationToken = default)
  {
    _dbContext.Jobs.Update(job);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return job;
  }
}