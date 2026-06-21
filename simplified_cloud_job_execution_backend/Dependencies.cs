using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.Ssm;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;
using simplified_cloud_job_execution_backend.Infrastructure.Database;
using simplified_cloud_job_execution_backend.Infrastructure.Execution;
using simplified_cloud_job_execution_backend.Repositories.JobRepository;
using simplified_cloud_job_execution_backend.Services.BillingServices;
using simplified_cloud_job_execution_backend.Services.JobServices;
using simplified_cloud_job_execution_backend.Exceptions;

namespace simplified_cloud_job_execution_backend;

public static class Dependencies
{
  public static void Inject(this IServiceCollection services, IConfiguration configuration)
  {
    AddEndpoints(services);
    AddInfrastructure(services, configuration);
    AddCustomService(services);
    AddGlobalExceptionHandler(services);
  }

  private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options =>
    {
      options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
      options.UseSnakeCaseNamingConvention();
    });

    services.AddDefaultAWSOptions(configuration.GetAWSOptions());

    services.AddAWSService<IAmazonS3>();
    services.AddAWSService<IAmazonSQS>();
    services.AddAWSService<IAmazonSimpleSystemsManagement>();

    services.Configure<SqsOptions>(configuration.GetSection(SqsOptions.SectionName));
    services.Configure<S3Options>(configuration.GetSection(S3Options.SectionName));
    services.Configure<SsmOptions>(configuration.GetSection(SsmOptions.SectionName));
  }

  private static void AddEndpoints(this IServiceCollection services)
  {
    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  private static void AddGlobalExceptionHandler(this IServiceCollection services)
  {
    services.AddProblemDetails();
    services.AddExceptionHandler<GlobalExceptionHandler>();
  }

  private static void AddCustomService(this IServiceCollection services)
  {
    services.AddScoped<IJobServices, JobServices>();
    services.AddScoped<IJobRepository, JobRepository>();
    services.AddScoped<IJobQueue, SqsJobQueue>();

    services.AddScoped<ISsmCommandService, SsmCommandService>();
    services.AddScoped<IJobExecutor, Ec2JobExecutor>();
    services.AddScoped<IS3FileService, S3FileService>();

    services.AddHostedService<JobExecutionWorker>();

    services.AddScoped<IBillingService, BillingService>();
  }
}