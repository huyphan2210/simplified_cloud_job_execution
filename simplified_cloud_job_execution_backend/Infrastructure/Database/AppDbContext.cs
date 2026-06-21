using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

  public DbSet<Job> Jobs => Set<Job>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    var jobBuilder = modelBuilder.Entity<Job>();

    jobBuilder.ToTable("Jobs");

    jobBuilder.HasKey(x => x.Id);

    jobBuilder.Property(x => x.JobName)
        .IsRequired()
        .HasMaxLength(200);

    jobBuilder.Property(x => x.ProjectId)
        .IsRequired()
        .HasMaxLength(100);

    jobBuilder.Property(x => x.InputFileName)
        .IsRequired();

    jobBuilder.Property(x => x.OutputFileReference);

    jobBuilder.Property(x => x.Status)
        .HasConversion<string>();

    jobBuilder.Property(x => x.ComputeType)
        .HasConversion<string>();
  }
}