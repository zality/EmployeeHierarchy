using System.Collections.Generic;

namespace EmployeeHierarchy
{
    public class Employee : ValidationBase
    {
        public Employee()
        {
            this.ValidationErrors = new List<ValidationError>();
        }

        public Employee(string id, string manager, int salary) : this()
        {
            this.Id = id;
            this.ManagerId = manager;
            this.Salary = salary;
        }

        public string Id { get; set; }

        public string ManagerId { get; set; }

        public int Salary { get; set; }

    }
}
