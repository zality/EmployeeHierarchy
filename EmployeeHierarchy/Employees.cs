using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeHierarchy
{
    public class Employees : ValidationBase
    {
        #region Public Properties

        public List<Employee> EmployeeList { get; private set; }

        #endregion Properties

        #region Constructors

        public Employees()
        {
            this.ValidationErrors = new List<ValidationError>();
        }

        public Employees(string employeeCSVData) : this()
        {
            EmployeeList = ValidateAndCreateEmployeeList(employeeCSVData);

            if (IsValid)
            {
                ValidateOnlyOneCEO(EmployeeList);
            }


            if (IsValid)
            {
                NoManagerThatIsNotAnEmployee(EmployeeList);
            }
        }

        #endregion Constructors

        #region Methods

        private List<Employee> ValidateAndCreateEmployeeList(string employeeCSVData)
        {
            if (string.IsNullOrEmpty(employeeCSVData))
            {
                this.ValidationErrors.Add(new ValidationError("Invalid csv.", nameof(Employees)));
                return null;
            }
            var employees = new List<Employee>();

            var rows = employeeCSVData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var row in rows)
            {
                var employee = new Employee();
                employees.Add(employee);

                var columns = row.Split(',');
                //validate row
                if (columns.Length != 3)
                {
                    employee.ValidationErrors.Add(new ValidationError("Malformed employee row, invalid number of columns.", nameof(Employee)));
                    continue; //this failure should stop continuation of populating this record
                }

                //Validate the employee Id
                var id = columns[0];
                if (string.IsNullOrEmpty(id))
                {
                    employee.ValidationErrors.Add(new ValidationError("Employee Id is a required field.", nameof(Employee.Id)));
                }
                else
                {
                    employee.Id = id;
                }

                //Validate the manager Id
                //No validation for column
                employee.ManagerId = columns[1];

                //Validate the salary
                var salary = columns[2];
                if (string.IsNullOrEmpty(salary))
                {
                    employee.ValidationErrors.Add(new ValidationError("Salary is a required field.", nameof(Employee.Salary)));
                }
                else
                {
                    var intSalary = 0;
                    if (!int.TryParse(salary, out intSalary))
                    {
                        employee.ValidationErrors.Add(new ValidationError("Invalid type, Salary is an integer field.", nameof(Employee.Salary)));
                    }
                    else
                    {
                        employee.Salary = intSalary;
                    }
                }

                //Validate the hierarchy
                {
                    var duplicateEmployees = employees.Where(p => p.Id == employee.Id);
                    if (duplicateEmployees.Count() > 1)
                    {
                        var groupedEmployees = duplicateEmployees.GroupBy(p => p.ManagerId);
                        if (groupedEmployees.Count() > 1)
                        {
                            employee.ValidationErrors.Add(new ValidationError("Employee hierarchy is invalid.", nameof(Employee.ManagerId)));
                        }
                    }
                }


                //Validate no circular reference
                {
                    var employeeManager = employees.FirstOrDefault(p => p.Id == employee.ManagerId);
                    if (employeeManager != null)
                    {
                        if (employeeManager.ManagerId == employee.Id)
                        {
                            employee.ValidationErrors.Add(new ValidationError("Circular management reference found.", nameof(Employee.ManagerId)));
                        }
                    }
                }
            }

            return employees;
        }

        private void ValidateOnlyOneCEO(List<Employee> employeeList)
        {
            try
            {
                var onlyOneCEO = employeeList.SingleOrDefault(p => string.IsNullOrEmpty(p.ManagerId)); // this will throw an exception if there are more than one result
            }
            catch (InvalidOperationException ex)
            {
                this.ValidationErrors.Add(new ValidationError("There are more than one CEOs.", nameof(Employee)));
            }
        }

        private void NoManagerThatIsNotAnEmployee(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                if (!string.IsNullOrEmpty(employee.ManagerId)) //not a manager
                {
                    var manager = employeeList.FirstOrDefault(p => p.Id == employee.ManagerId);
                    if (manager == null)
                    {
                        this.ValidationErrors.Add(new ValidationError("There are managers that are not employees.", nameof(Employee)));
                        return;
                    }
                }
            }
        }

        public long GetManagerSalaryBudget(string managerId)
        {
            var subordinates = EmployeeList.Where(p => p.ManagerId == managerId);
            var subordinatesSalary = subordinates.Sum(p => p.Salary);

            //Manager Salary
            var manager = EmployeeList.FirstOrDefault(p => p.Id == managerId);
            if (manager != null)
            {
                subordinatesSalary += manager.Salary;
            }

            return subordinatesSalary;
        }

        #endregion
    }
}
