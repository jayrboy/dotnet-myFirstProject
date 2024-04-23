using myFirstProject.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace myFirstProject.Models
{
    public class EmployeeMetadata
    {

    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        //Create Action
        public static Employee Create(EmployeeContext db, Employee employee)
        {
            employee.CreateDate = DateTime.Now;
            employee.UpdateDate = DateTime.Now;
            employee.IsDelete = false;
            db.Employees.Add(employee);
            db.SaveChanges();

            return employee;
        }

        //Get All Action
        public static List<Employee> GetAll(EmployeeContext db)
        {
            // List<Employee> result = db.Employees.Where(q => q.IsDelete != true).ToList();
            List<Employee> result = db.Employees.Include(i => i.Department).Where(q => q.IsDelete != true).ToList();
            return result;
        }

        //Get ID Action
        public static Employee GetById(EmployeeContext db, int id)
        {
            Employee? result = db.Employees.Include(i => i.Department).Where(q => q.Id == id && q.IsDelete != true).FirstOrDefault();
            return result ?? new Employee();
        }

        //Update Action
        public static Employee Update(EmployeeContext db, Employee employee)
        {
            employee.UpdateDate = DateTime.Now;
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();

            return employee;
        }

        //Delete Action
        public static Employee Delete(EmployeeContext db, int id)
        {
            Employee employee = GetById(db, id);

            employee.IsDelete = true;
            db.Entry(employee).State = EntityState.Modified;

            // db.Employees.Remove(employee);

            db.SaveChanges();
            return employee;
        }

        //Search Action
        public static List<Employee> Search(EmployeeContext db, string keyword)
        {
            List<Employee> result = db.Employees.Where(q => q.Firstname.Contains(keyword) || q.Lastname.Contains(keyword) && q.IsDelete != true).ToList();
            return result;
        }

    }
}
