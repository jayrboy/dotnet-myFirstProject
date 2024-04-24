using System.ComponentModel.DataAnnotations;
using myFirstProject.Data;



namespace myFirstProject.Models
{
    public class ProjectMetaData
    {

    }

    [MetadataType(typeof(ProjectMetaData))]
    public partial class Project
    {
        public static Project Create(EmployeeContext db, Project project)
        {
            project.CreateDate = DateTime.Now;
            project.UpdateDate = DateTime.Now;
            project.IsDelete = false;
            db.Projects.Add(project);
            db.SaveChanges();
            return project;
        }

        public static Project Update(EmployeeContext db, Project project)
        {
            project.UpdateDate = DateTime.Now;
            db.Projects.Update(project);
            db.SaveChanges();
            return project;
        }

        public static Project Get(EmployeeContext db, int id)
        {
            Project? project = db.Projects.Where(e => e.Id == id && e.IsDelete != true).FirstOrDefault();
            return project ?? new Project();
        }

        public static List<Project> GetAll(EmployeeContext db)
        {
            List<Project> projects = db.Projects.Where(e => e.IsDelete != true).ToList();
            return projects;
        }

        public static Project Delete(EmployeeContext db, Project project)
        {
            project.IsDelete = true;
            db.Projects.Update(project);
            db.SaveChanges();
            return project;
        }
    }

}



