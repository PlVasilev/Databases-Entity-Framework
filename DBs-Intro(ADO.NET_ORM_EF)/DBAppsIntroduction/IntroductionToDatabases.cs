using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

//ADO.NET

namespace DBCsharp
{
    class DbAppsIntroduction
    {
        static void Main(string[] args)
        {
            //          Creating and opening connection to SQL Server (database SoftUni)
             SqlConnection dbCon = new SqlConnection(
                 "Server=.; " +
                 "Database=SoftUni; " +
                 "Integrated Security=true");
             dbCon.Open();
             using (dbCon)
             {
                 SqlCommand command = new SqlCommand(
                     "SELECT COUNT(*) FROM Employees", dbCon); // SQL command
                 int employeesCount = (int)command.ExecuteScalar(); // cast everything from SQL Command
                 Console.WriteLine("Employees count: {0} ", employeesCount);
             }

           //          The SqlDataReader Class
             SqlConnection dbCon2 = new SqlConnection("Server=.; " +
                                                      "Database=SoftUni; " +
                                                      "Integrated Security=true");
             dbCon.Open();
             using (dbCon2)
             {
                 SqlCommand command = new SqlCommand("SELECT * FROM Employees", dbCon);
                 SqlDataReader reader = command.ExecuteReader();
                 using (reader)
                 {
                     while (reader.Read())
                     {
                         string firstName = (string)reader["FirstName"];
                         string lastName = (string)reader["LastName"];
                         decimal salary = (decimal)reader["Salary"];
                         Console.WriteLine("{0} {1} - {2}", firstName, lastName, salary);
                     }
                 }
             }

            //What is SQL Injection? (1)
           //bool IsPasswordValid(string username, string password)
           //{
           //    string sql =
           //        $"SELECT COUNT(*) FROM Users " +
           //        $"WHERE UserName = '{username}' AND" +
           //        $"PasswordHash = '{CalcSHA1(password)}'";
           //    SqlCommand cmd = new SqlCommand(sql, dbConnection);
           //
           //    int matchedUsersCount = (int)cmd.ExecuteScalar();
           //    return matchedUsersCount > 0;
           //}
           //
           //bool normalLogin =
           //    IsPasswordValid("peter", "qwerty123"); // true
           //
           //bool sqlInjectedLogin =
           //    IsPasswordValid(" ' or 1=1 --", "qwerty123"); // true
           //
           //bool evilHackerCreatesNewUser =
           //    IsPasswordValid("' INSERT INTO Users VALUES('hacker','') --",
           //        "qwerty123");

            //              How Does SQL Injection Work?
            //The following SQL commands are executed:
            //Usual password check(no SQL injection):
            //SELECT COUNT(*) FROM Users WHERE UserName = 'peter’AND PasswordHash = 'XOwXWxZePV5iyeE86Ejvb + rIG / 8 = '
            //SQL-injected password check:
            //SELECT COUNT(*) FROM Users WHERE UserName = ' ' or 1=1-- ‘ AND PasswordHash = 'XOwXWxZePV5iyeE86Ejvb+rIG/8='
            //SQL-injected INSERT command:
            //SELECT COUNT(*) FROM Users WHERE UserName = ''INSERT INTO Users VALUES('hacker','')--’ AND PasswordHash = 'XOwXWxZePV5iyeE86Ejvb+rIG/8='

            //              Parameterized Commands – Example
            void InsertProject(string name, string description, DateTime startDate)
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Projects " +
                    "(Name, Description, StartDate, EndDate) VALUES " +
                    "(@name, @desc, @start, @end)", dbCon);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@start", startDate);

                cmd.ExecuteNonQuery();
            }

        }
    }
}
