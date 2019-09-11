using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EmployeeHierarchy.Tests
{
    [TestClass()]
    public class EmployeesTests
    {
        [TestMethod()]
        public void EmployeesTest_ValidateConstructorRequiresANonEmpltyString()
        {
            var employees = new Employees(null);
            Assert.IsFalse(employees.IsValid);
        }

        /// <summary>
        /// Test for question 1 in the pdf
        /// </summary>
        [TestMethod()]
        public void EmployeesTest_SalariesAreValidIntegerNumbers()
        {
            string inValidSalaryData = $"Employee4,Employee2,invalid{Environment.NewLine}Employee3,Employee1,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";
            var invalidEmployees = new Employees(inValidSalaryData);
            var invalidEmployee = invalidEmployees.EmployeeList.First();

            Assert.IsTrue(invalidEmployee.ValidationErrorMessage.Contains("Invalid type, Salary is an integer field"));
        }

        /// <summary>
        /// Test for question 2 in the pdf
        /// The assumption is that you can have 2 entries of the same employee in the csv, listed under different managers
        /// </summary>
        [TestMethod()]
        public void EmployeesTest_OneEmployeeDoesNotReportToMoreThanOneManager()
        {
            string inValidManagerData = $"Employee4,Employee2,500{Environment.NewLine}Employee4,Employee1,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";
            var invalidEmployees = new Employees(inValidManagerData);
            var invalidEmployee = invalidEmployees.EmployeeList.Where(p => p.ValidationErrorMessage.Contains("Employee hierarchy is invalid."));
            Assert.IsNotNull(invalidEmployee);
        }


        /// <summary>
        /// Test for question 3 in the pdf
        /// </summary>
        [TestMethod()]
        public void EmployeesTest_OnlyOneCEO()
        {
            string inValidCEOData = $"Employee4,Employee2,500{Environment.NewLine}Employee3,,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";
            var invalidEmployees = new Employees(inValidCEOData);
            var istrue = invalidEmployees.ValidationErrorMessage.Contains("There are more than one CEOs.");
            Assert.IsTrue(istrue);
        }

        /// <summary>
        /// Test for question 4 in the pdf
        /// </summary>
        [TestMethod()]
        public void EmployeesTest_NoCircularReference()
        {
            string inValidManagerData = $"Employee4,Employee3,500{Environment.NewLine}Employee3,Employee4,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";
            var invalidEmployees = new Employees(inValidManagerData);
            var invalidEmployee = invalidEmployees.EmployeeList.Where(p => p.ValidationErrorMessage.Contains("Circular management reference found."));
            Assert.IsNotNull(invalidEmployee);
        }


        /// <summary>
        /// Test for question 5 in the pdf
        /// </summary>
        [TestMethod()]
        public void EmployeesTest_NoManagerThatIsNotAnEmployee()
        {
            string inValidManagerData = $"Employee4,Employee6,500{Environment.NewLine}Employee3,Employee2,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";
            var invalidEmployees = new Employees(inValidManagerData);
            var istrue = invalidEmployees.ValidationErrorMessage.Contains("There are managers that are not employees.");
            Assert.IsTrue(istrue);
        }

        [TestMethod]
        public void EmployeesTest_ManagerSalaryBudget()
        {
            string validData = $"Employee4,Employee1,500{Environment.NewLine}Employee3,Employee2,800{Environment.NewLine}Employee1,,1000{Environment.NewLine}Employee5,Employee1,500{Environment.NewLine}Employee2,Employee1,500";

            var validEmployees = new Employees(validData);
            var managerBudget = validEmployees.GetManagerSalaryBudget("Employee1");
            Assert.AreEqual(2500, managerBudget); //Employee1 salary budget is 2500
        }
    }
}