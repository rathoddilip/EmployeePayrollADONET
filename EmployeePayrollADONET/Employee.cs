using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollADONET
{
    public class Employee
    {
        public int id;
        public string name;
        public DateTime startDate;
        public char gender;
        public long phoneNumber;
        public string address;
        public string[] department;
        public float basicPay;
        public int deduction;
        public int taxablePay;
        public int incomeTax;
        public int netPay;
        public string companyName;

        public Employee()
        {
            department = new string[10];
        }
    }
}
