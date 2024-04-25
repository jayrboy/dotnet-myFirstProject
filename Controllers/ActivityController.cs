using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Models;


[ApiController]
[Route("[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Create Activity
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// ```json
    /// [
    ///     {
    ///         "name": "Act1",
    ///         "projectId": 2,
    ///         "inverseActivityHeader": [
    ///             {
    ///                 "name": "Act1.1",
    ///                 "projectId": 2
    ///             }
    ///         ]
    ///     },
    ///     {
    ///         "name": "Act2",
    ///         "projectId": 2,
    ///         "inverseActivityHeader": [
    ///             {
    ///                 "name": "Act2.1",
    ///                 "projectId": 2
    ///             },
    ///             {
    ///                 "name": "Act2.2",
    ///                 "projectId": 2,
    ///                 "inverseActivityHeader": [
    ///                     {
    ///                         "name": "Act2.2.1",
    ///                         "projectId": 2
    ///                     }
    ///                 ]
    ///             }
    ///         ]
    ///     }
    /// ]
    /// ```
    /// </remarks>
    /// <param name="activitiesRequest"></param>
    /// <returns></returns>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
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