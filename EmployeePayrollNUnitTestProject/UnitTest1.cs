using EmployeePayrollADONET;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EmployeePayrollNUnitTestProject
{
    public class Tests
    {
        EmpPayroll empPayroll;
        [SetUp]
        public void Setup()
        {
            empPayroll = new EmpPayroll();
        }
        [Test]
        public void GivenUpdatedSalaryByName_UpdateSalary()
        {
            empPayroll.UpdateEmployeeSalary("Sandip", 300000);
            List<Employee> employees = empPayroll.ReturnAllRecords();
            Employee updateDetails = new Employee();
            foreach (Employee employee in employees)
            {
                if (employee.name == "Sandip")
                {
                    updateDetails = employee;
                    break;
                }
            }
            Assert.AreEqual(300000, updateDetails.basicPay);
        }

    }
}