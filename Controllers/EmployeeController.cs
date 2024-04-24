using System.IO.Packaging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;
using OfficeOpenXml;
using Xceed.Document.NET;
using Xceed.Words.NET;

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

    //Export Word
    [HttpGet("export/word", Name = "ExportEmployeeToWord")]
    public IActionResult ExportEmployeeToWord()
    {
        List<Employee> employees = Employee.GetAll(_db).ToList();

        //Create a new document
        using (DocX document = DocX.Create("SampleDocument.docx"))
        {
            //Add a table to the document
            document.InsertParagraph("List of employees").FontSize(18).Bold().Alignment = Alignment.center;

            //Add a title to the document
            Table table = document.AddTable(1, 4);
            table.Design = TableDesign.ColorfulList;
            table.Alignment = Alignment.center;
            table.AutoFit = AutoFit.Contents;

            //Add headers to the document
            table.Rows[0].Cells[0].Paragraphs[0].Append("ID").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Firstname").Bold();
            table.Rows[0].Cells[2].Paragraphs[0].Append("Lastname").Bold();
            table.Rows[0].Cells[3].Paragraphs[0].Append("Salary").Bold();

            //Add data to the table
            for (int i = 0; i < employees.Count; i++)
            {
                table.InsertRow();
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append(employees[i].Id.ToString());
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append(employees[i].Firstname);
                table.Rows[i + 1].Cells[2].Paragraphs[0].Append(employees[i].Lastname);
                table.Rows[i + 1].Cells[3].Paragraphs[0].Append(employees[i].Salary.ToString());
            }

            document.InsertTable(table);

            //Save the document to a memory stream
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            document.SaveAs(stream);

            //Reset the stream position
            stream.Position = 0;

            //Set the content type and file name for the response
            string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = "SampleDocument.docx";

            //Return the file content as a file result
            return File(stream, contentType, fileName);
        }
    }

    //Export Excel
    [HttpGet("export/excel", Name = "ExportEmployeeToExcel")]
    public IActionResult ExportEmployeeToExcel()
    {
        List<Employee> employees = Employee.GetAll(_db).ToList();

        //Create a new Excel package
        using (ExcelPackage package = new ExcelPackage())
        {
            //Create a worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employees");

            //Set the column headers and decorate them with styles
            var headerCells = worksheet.Cells["A1:D1"];
            headerCells.Style.Font.Bold = true;
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            headerCells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //Set the column headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Firstname";
            worksheet.Cells[1, 3].Value = "lastname";
            worksheet.Cells[1, 4].Value = "Salary";

            //Add data to the worksheet
            int row = 2;
            foreach (var employee in employees)
            {
                worksheet.Cells[row, 1].Value = employee.Id;
                worksheet.Cells[row, 2].Value = employee.Firstname;
                worksheet.Cells[row, 3].Value = employee.Lastname;
                worksheet.Cells[row, 4].Value = employee.Salary;

                //Apply cell border
                worksheet.Cells[row, 1, row, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                row++;
            }

            //Auto-fit the column
            worksheet.Cells.AutoFitColumns();

            //Convert the Excel package to a byte array
            byte[] excelBytes = package.GetAsByteArray();

            //Set the content type and file name for the response
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Employees.xlsx";

            //Return the Excel file as a fle result
            return File(excelBytes, contentType, fileName);
        }
    }


}