using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;


[ApiController]
[Route("[controller]")]

public class ActivityController : ControllerBase
{
    private EmployeeContext _db = new EmployeeContext();

    private readonly ILogger<ActivityController> _logger;

    public ActivityController(ILogger<ActivityController> logger)
    {
        _logger = logger;
    }

    public partial class ActivityCreateRequest
    {
        public int? ProjectId { get; set; }
        public int? ActivityHeaderId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Activity> InverseActivityHeader { get; set; } = new List<Activity>();
    }

    //Create
    [HttpPost("Create", Name = "CreateActivity")]
    public ActionResult<Response> Create(List<ActivityCreateRequest> activitiesRequest)
    {
        List<Activity> activities = new List<Activity>();
        activities = activitiesRequest.Select(a => new Activity
        {
            ProjectId = a.ProjectId,
            ActivityHeaderId = a.ActivityHeaderId,
            Name = a.Name,
            InverseActivityHeader = a.InverseActivityHeader
        }).ToList();
        try
        {
            foreach (Activity activity in activities)
            {
                Activity.SetActivitiesCreate(activity);
                Activity.Create(_db, activity);
            }
            _db.SaveChanges();
            return new Response
            {
                Code = 200,
                Message = "Success",
                Data = activities
            };
        }
        catch
        {
            return new Response
            {
                Code = 500,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }



}