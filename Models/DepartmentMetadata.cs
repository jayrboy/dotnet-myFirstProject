using myFirstProject.Data;
using System.ComponentModel.DataAnnotations;


namespace myFirstProject.Models
{
    public class DepartmentMetadata
    {

    }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {
        //Create Action
        public static Department Create(EmployeeContext db, Department department)
        {
            department.CreateDate = DateTime.Now;
            department.UpdateDate = DateTime.Now;
            department.IsDelete = false;
            db.Departments.Add(department);
            db.SaveChanges();

            return department;
        }

        //Get All Action
        public static List<Department> GetAll(EmployeeContext db)
        {
            List<Department> result = db.Departments.Where(q => q.IsDelete != true).ToList();
            return result;
        }


    }
}
