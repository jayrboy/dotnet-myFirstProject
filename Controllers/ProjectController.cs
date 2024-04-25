using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProjectController : ControllerBase
{
    private EmployeeContext _db = new EmployeeContext();

    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    public struct ProjectCreate
    {
        public string? Name { get; set; }

        /// <summary>
        /// StartDate of the Project
        /// </summary>
        /// <example>2024-01-01</example>
        /// <required>true</required>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// EndDate of the Project
        /// </summary>
        /// <example>2024-01-31</example>
        /// <required>true</required>
        public DateOnly? EndDate { get; set; }
    }

    /// <summary>
    /// Create Project
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// ```json
    /// {
    ///     "name": "Project123",
    ///     "startDate": "2022-01-01",
    ///     "endDate": "2022-01-31"
    /// }
    /// ```
    /// </remarks>
    /// <param name="projectCreate"></param>
    /// <returns></returns>
    [HttpPost(Name = "CreateProject")]
    public ActionResult CreateProject(ProjectCreate projectCreate)
    {
        Project project = new Project
        {
            Name = projectCreate.Name,
            StartDate = projectCreate.StartDate,
            EndDate = projectCreate.EndDate,
        };
        project = Project.Create(_db, project);
        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = project
        });
    }

}