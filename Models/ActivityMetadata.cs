using System.ComponentModel.DataAnnotations;
using myFirstProject.Data;

namespace myFirstProject.Models
{
    public class ActivityMetadata { }

    [MetadataType(typeof(ActivityMetadata))]

    public partial class Activity
    {
        public static Activity Create(EmployeeContext db, Activity activity)
        {
            activity.CreateDate = DateTime.Now;
            activity.UpdateDate = DateTime.Now;
            activity.IsDelete = false;
            db.Activities.Add(activity);

            return activity;
        }

        public static void SetActivitiesCreate(Activity activity)
        {
            activity.CreateDate = DateTime.Now;
            activity.UpdateDate = DateTime.Now;
            activity.IsDelete = false;
            foreach (Activity subActivity in activity.InverseActivityHeader)
            {
                SetActivitiesCreate(subActivity);
            }
        }
    }
}


