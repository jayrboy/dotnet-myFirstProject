using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;

[ApiController]
[Route("[controller]")]

public class EmployeeController : ControllerBase
{
    private EmployeeContext _db = new EmployeeContext();

    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger)
    {
        _logger = logger;
    }

    public struct EmployeeCreate
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int? Salary { get; set; }
        public int? DepartmentId { get; set; }
    }

    //Create
    [HttpPost(Name = "CreateEmployee")]
    public ActionResult CreateEmployee(EmployeeCreate employeeCreate)
    {
        Employee employee = new Employee
        {
            Firstname = employeeCreate.Firstname,
            Lastname = employeeCreate.Lastname,
            Salary = employeeCreate.Salary,
            DepartmentId = employeeCreate.DepartmentId,
        };

        employee = Employee.Create(_db, employee);
        return Ok(employee);
    }

    //Get All
    [HttpGet(Name = "GetAllEmployee")]
    public ActionResult GetAllEmployee()
    {
        // List<Employee> employees = Employee.GetAll(_db).OrderBy(q => q.Salary).ToList(); // น้อย -> มาก
        List<Employee> employees = Employee.GetAll(_db).OrderByDescending(q => q.Salary).ToList(); // มาก -> น้อย
        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = employees
        });
    }

    //GetById
    [HttpGet("{id}", Name = "GetEmployeeById")]
    public ActionResult GetEmployeeById(int id)
    {
        Employee employees = Employee.GetById(_db, id);
        return Ok(employees);
    }

    //Update
    [HttpPut(Name = "UpdateEmployee")]
    public ActionResult UpdateEmployee(Employee employee)
    {
        employee = Employee.Update(_db, employee);
        return Ok(employee);
    }

    [HttpDelete("{id}", Name = "DeleteEmployeeById")]
    public ActionResult DeleteEmployeeById(int id)
    {
        Employee employee = Employee.Delete(_db, id);
        return Ok(employee);
    }

    //Search By Name
    [HttpGet("/search/{name}", Name = "SearchEmployeeByName")]
    public ActionResult SearchEmployeeByName(string name)
    {
        List<Employee> employees = Employee.Search(_db, name);
        if (employees.Count == 0)
        {
            return NotFound(new Response
            {
                Code = 404,
                Message = "Employees not found",
                Data = null,
            });
        }

        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = employees
        });
    }

    //Get All By Page
    [HttpGet("page/{page}", Name = "GetAllEmployeeByPage")]
    public ActionResult GetAllEmployeeByPage(int page)
    {
        int pageSize = 3;
        List<Employee> employees = Employee.GetAll(_db).OrderByDescending(q => q.Salary).Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = employees
        });

    }
}