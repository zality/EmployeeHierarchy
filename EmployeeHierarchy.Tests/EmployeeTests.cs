using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeHierarchy.Tests
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void Employee_ConstructorTest()
        {
            var x = new Employee("Employee1", "", 500);
            Assert.IsNotNull(x);
        }
    }
}
