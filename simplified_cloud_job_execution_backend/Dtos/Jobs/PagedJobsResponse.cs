using System.Collections.Generic;

namespace simplified_cloud_job_execution_backend.Dtos.Jobs;

public class PagedJobsResponse
{
  public IReadOnlyList<GetJobStatusResponse> Jobs { get; set; } = Array.Empty<GetJobStatusResponse>();
  public int CurrentPage { get; set; }
  public int TotalPage { get; set; }
  public int PageSize { get; set; }
}
