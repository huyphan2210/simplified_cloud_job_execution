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

  public async Task<(IReadOnlyList<Job> Jobs, int TotalCount, int CurrentPage, int CurrentChunk)> GetPagedJobsAsync(int page, int chunk, CancellationToken cancellationToken = default)
  {
    if (page < 1)
    {
      page = 1;
    }

    if (chunk < 1)
    {
      chunk = 10;
    }

    var query = _dbContext.Jobs
      .Where(job => job.IsDeleted != true);

    var totalCount = await query.CountAsync(cancellationToken);
    var jobs = await query
      .OrderByDescending(job => job.CreatedAt)
      .Skip((page - 1) * chunk)
      .Take(chunk)
      .ToListAsync(cancellationToken);

    return (jobs, totalCount, page, chunk);
  }

  public async Task<Job> UpdateJobAsync(Job job, CancellationToken cancellationToken = default)
  {
    _dbContext.Jobs.Update(job);
    await _dbContext.SaveChangesAsync(cancellationToken);
    return job;
  }
}