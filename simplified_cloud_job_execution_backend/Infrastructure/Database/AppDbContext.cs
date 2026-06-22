using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Domain.Projects;

namespace simplified_cloud_job_execution_backend.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureJobs(modelBuilder);
        ConfigureProjects(modelBuilder);
    }

    private static void ConfigureJobs(ModelBuilder modelBuilder)
    {
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

    private static void ConfigureProjects(ModelBuilder modelBuilder)
    {
        var projectBuilder = modelBuilder.Entity<Project>();

        projectBuilder.ToTable("Projects");
        projectBuilder.HasKey(x => x.Id);

        projectBuilder.Property(x => x.ProjectName)
            .IsRequired()
            .HasMaxLength(200);

        projectBuilder.Property(x => x.Description)
            .HasMaxLength(500);

        projectBuilder.Property(x => x.CreatedAt)
            .IsRequired();

        projectBuilder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        projectBuilder.HasData(
            new Project
            {
                Id = Guid.Parse("9a3e8f89-0fd2-4f42-84c5-2b4a4aa232c1"),
                ProjectName = "Inference Lab",
                Description = "GPU-backed model inference and validation workloads.",
                CreatedAt = new DateTime(2026, 1, 10, 8, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
            },
            new Project
            {
                Id = Guid.Parse("af23b61d-4d7e-4f58-9f38-6ccaf68fca74"),
                ProjectName = "Analytics Pipeline",
                Description = "Batch ETL and analytics exports for engineering metrics.",
                CreatedAt = new DateTime(2026, 2, 2, 9, 30, 0, DateTimeKind.Utc),
                IsDeleted = false,
            },
            new Project
            {
                Id = Guid.Parse("d41f4b5a-5c8a-4dbe-a5d6-8f3a923b0d57"),
                ProjectName = "Media Rendering",
                Description = "Video and media rendering jobs for quality checks.",
                CreatedAt = new DateTime(2026, 3, 15, 12, 15, 0, DateTimeKind.Utc),
                IsDeleted = false,
            },
            new Project
            {
                Id = Guid.Parse("f9e8b0a0-1c47-4472-bc7a-2e8e473cd8af"),
                ProjectName = "IoT Batch Jobs",
                Description = "Sensor aggregation and device telemetry processing.",
                CreatedAt = new DateTime(2026, 4, 4, 10, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
            },
            new Project
            {
                Id = Guid.Parse("3c9b2b6d-2a71-4023-8f9d-ea1b48a61520"),
                ProjectName = "ML Training",
                Description = "Model training and hyperparameter sweep workloads.",
                CreatedAt = new DateTime(2026, 5, 5, 14, 45, 0, DateTimeKind.Utc),
                IsDeleted = false,
            }
        );
    }
}
