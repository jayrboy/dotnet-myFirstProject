using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;

namespace myFirstProject.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private EmployeeContext _db = new EmployeeContext();
    private IHostEnvironment _hostingEnvironment;

    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _hostingEnvironment = environment;
    }

    /// <summary>
    /// Upload
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    [HttpPost(Name = "UploadFile")]
    public ActionResult UploadFile(IFormFile formFile)
    {
        if (formFile == null)
        {
            return BadRequest(new Response
            {
                Code = 400,
                Message = "File is required"
            });
        }

        Models.File file = new Models.File
        {
            FileName = formFile.FileName,
            FilePath = "UploadedFile/ProfileImg/"
        };
        file = Models.File.Create(_db, file);

        if (formFile != null && formFile.Length > 0)
        {
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "UploadedFile/ProfileImg/" + file.Id);
            // string filePath = Path.Combine(Server.MapPath("~/UploadedFile/Profile/") + AccountExtensions.File.Id + "/" + file.FileName);

            Directory.CreateDirectory(uploads);
            string filePath = Path.Combine(uploads, formFile.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }
        }

        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = file
        });
    }

}