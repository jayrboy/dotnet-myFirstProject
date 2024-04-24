using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;

[ApiController]
[Route("[controller]")]

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
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }

    //Create
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