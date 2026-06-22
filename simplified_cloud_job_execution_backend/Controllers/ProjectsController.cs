using Microsoft.AspNetCore.Mvc;
using simplified_cloud_job_execution_backend.Dtos.Projects;
using simplified_cloud_job_execution_backend.Services.ProjectServices;

namespace simplified_cloud_job_execution_backend.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController(IProjectServices projectServices, ILogger<ProjectsController> logger) : ControllerBase
{
  private readonly IProjectServices _projectServices = projectServices;
  private readonly ILogger<ProjectsController> _logger = logger;

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectResponse>))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetAllProjects()
  {
    var projects = await _projectServices.GetAllProjectsAsync();
    return Ok(projects);
  }
}
