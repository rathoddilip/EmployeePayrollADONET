using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmployeePayrollADONET
{
    public class EmpPayroll
    {
        static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=payroll_service;User Id=root;Password=root";

        public List<Employee> ReturnAllRecords()
        {
            List<Employee> employees = new List<Employee>();
            Employee emp = new Employee();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "GetEmployeePayrollAllData";
            using (connection)
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader dr = sqlCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        emp.id = dr.GetInt32(0);
                        emp.name = dr.GetString(1);
                        emp.startDate = dr.GetDateTime(3).Date;
                        emp.gender = Convert.ToChar(dr.GetString(4));
                        emp.phoneNumber = dr.IsDBNull(5) ? 0 : dr.GetInt64(5);
                        emp.address = dr.IsDBNull(7) ? "" : dr.GetString(7);
                        emp.department[0] = dr.GetString(6);
                        emp.basicPay = Convert.ToSingle(dr["basicPay"]);
                        //emp.basicPay = dr.IsDBNull(2) ? 0 : dr.GetInt32(2);
                        emp.deduction = dr.IsDBNull(8) ? 0 : dr.GetInt32(8);
                        emp.taxablePay = dr.IsDBNull(9) ? 0 : dr.GetInt32(9);
                        emp.incomeTax = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                        employees.Add(emp);
                        Console.WriteLine(emp.id + "," + emp.name + "," + emp.phoneNumber + " "+ emp.address + " "+emp.basicPay);
                    }//end while

                }//end if
                connection.Close();
            }//end using
            return employees;
        }

        public List<Employee> RetreiveFromDateRange(DateTime start, DateTime end)
        {
            List<Employee> employees = new List<Employee>();
            Employee emp = new Employee();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "sp_GetEmployeeRecordsBetweenDates";
            using (connection)
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@StartDate", start);
                sqlCommand.Parameters.AddWithValue("@EndDate", end);
                connection.Open();
                SqlDataReader dr = sqlCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        emp.id = dr.GetInt32(0);
                        emp.name = dr.GetString(1);
                        emp.startDate = dr.GetDateTime(2).Date;
                        Console.WriteLine($"{emp.id},{emp.name},{emp.startDate}");
                        employees.Add(emp);
                    }//end while

                }//end if
                connection.Close();
            }//end using
            return employees;
        }

        public DataSet RetrieveGenderComparisonInfo()
        {
            string storedProcedure = "sp_CompareGenderSum";
            SqlDataAdapter dadapter = new SqlDataAdapter(storedProcedure, connectionString);
            DataSet dset = new DataSet();
            dadapter.Fill(dset, "genderSum");
            return dset;

        }
        /// <summary>
        /// Update salary by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="salary"></param>
        public void UpdateEmployeeSalary(string name, int salary)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string storedProcedure = "sp_UpdateEmpSalaryUsingName";
            try
            {
                using (connection)
                {
                    SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@salary", salary);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    int recordsUpdated = sqlCommand.ExecuteNonQuery();
                    Console.WriteLine($"{recordsUpdated} no of record(s) updated");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InsertRecordStoredProcedure()
        {
            Employee emp = new Employee();
            Console.WriteLine("Enter Employee Name");
            emp.name = Console.ReadLine();
            emp.startDate = DateTime.Now.Date;
            Console.WriteLine("Enter Gender");
            emp.gender = Console.ReadLine()[0];
            emp.phoneNumber = 0;
            Console.WriteLine("Enter Salary");
            emp.basicPay = Convert.ToInt32(Console.ReadLine());

            SqlConnection connection = new SqlConnection(connectionString);
            string query = "sp_AddEmployeeEntry";
            try
            {
                using (connection)
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Name", emp.name);
                    sqlCommand.Parameters.AddWithValue("@Gender", emp.gender);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", emp.phoneNumber);
                    sqlCommand.Parameters.AddWithValue("@BasicPay", emp.basicPay);
                    sqlCommand.Parameters.AddWithValue("@StartDate", emp.startDate);
                    
                    
                   
                    
                    

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void UpdateRecord()
        {
            Employee emp = new Employee();
            Console.WriteLine("Enter Employee Name");
            emp.name = Console.ReadLine();
            emp.startDate = DateTime.Now;
            Console.WriteLine("Enter Phone");
            emp.phoneNumber = Convert.ToInt32(Console.ReadLine());

            SqlConnection connection = new SqlConnection(connectionString);
            string storedProcedure = "sp_UpdateEmployeePhoneNumber";
            try
            {
                using (connection)
                {
                    SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@phoneNumber", emp.phoneNumber);
                    sqlCommand.Parameters.AddWithValue("@name", emp.name);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteRecord()
        {

            Console.WriteLine("Enter Id To delete");

            int id = Convert.ToInt32(Console.ReadLine());

            SqlConnection connection = new SqlConnection(connectionString);
            string query1 = @"Delete from Company where id =" + id + ";";
            string query2 = @"Delete from Payroll where id =" + id + ";";
            string query3 = @"Delete from Department where id =" + id + ";";
            string query4 = @"Delete from EmployeePayroll where id =" + id + ";";
            try
            {
                using (connection)
                {
                    SqlCommand sqlCommand1 = new SqlCommand(query1, connection);
                    SqlCommand sqlCommand2 = new SqlCommand(query2, connection);
                    SqlCommand sqlCommand3 = new SqlCommand(query3, connection);
                    SqlCommand sqlCommand4 = new SqlCommand(query4, connection);
                    connection.Open();
                    sqlCommand1.ExecuteNonQuery();
                    sqlCommand2.ExecuteNonQuery();
                    sqlCommand3.ExecuteNonQuery();
                    sqlCommand4.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        public void DeleteRecordStoredProcedure()
        {
            Console.WriteLine("Enter Id To delete");

            int id = Convert.ToInt32(Console.ReadLine());

            SqlConnection connection = new SqlConnection(connectionString);
            string storedProcedure = @"sp_DeleteRecordForId";

            try
            {
                using (connection)
                {
                    SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

    }
}
