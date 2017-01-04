using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;
namespace ERPDomain.Concrete
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Employee> Employee
        {
            get { return context.Employee; }
        }
        public void SaveEmployee(Employee employee)
        {
            if (employee.ID == 0)
            {
                context.Employee.Add(employee);
            }
            else
            {
                context.Entry(employee).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public void DeleteEmployee(Employee employee)
        {
            context.Employee.Remove(employee);
            context.SaveChanges();
        }
    }
}
