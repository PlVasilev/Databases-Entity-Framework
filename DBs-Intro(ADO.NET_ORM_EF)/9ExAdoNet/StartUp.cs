using System;
using System.Data.SqlClient;
using System.Linq;

namespace _9ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
                string commandString = @"CREATE PROC usp_GetOlder @id INT
                                        AS
                                        UPDATE Minions
                                           SET Age += 1
                                         WHERE Id = @id";

                try
                {
                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    int id = int.Parse(Console.ReadLine());


                    using (SqlCommand command = new SqlCommand("EXEC usp_GetOlder @Id", connection))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("SELECT Name, Age FROM Minions WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader[0]} – {reader[1]} years old");
                            }
                        }
                    }
                }
               

            }
        }
    }
}
