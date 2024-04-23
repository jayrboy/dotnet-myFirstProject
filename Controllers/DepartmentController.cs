using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;

[ApiController]
[Route("[controller]")]

public class DepartmentController : ControllerBase
{
    private EmployeeContext _db = new EmployeeContext();

    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(ILogger<DepartmentController> logger)
    {
        _logger = logger;
    }

    public struct DepartmentCreate
    {
        public string? Name { get; set; }
    }

    //Create
    [HttpPost(Name = "CreateDepartment")]
    public ActionResult CreateDepartment(DepartmentCreate departmentCreate)
    {
        Department department = new Department
        {
            Name = departmentCreate.Name,
        };
        department = Department.Create(_db, department);
        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = department
        }
        );
    }

    //Get All Department
    [HttpGet(Name = "GetAllDepartments")]
    public ActionResult GetAllDepartments()
    {
        List<Department> departments = Department.GetAll(_db);
        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = departments
        });
    }
}