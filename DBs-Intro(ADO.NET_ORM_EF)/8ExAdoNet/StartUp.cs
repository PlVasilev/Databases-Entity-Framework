using System;
using System.Data.SqlClient;
using System.Linq;

namespace _8ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
                int[] ids = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                    .ToArray();

                string commandString = @" UPDATE Minions
                                            SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                            WHERE Id = @Id";

                for (int i = 0; i < ids.Length; i++)
                {
                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        command.Parameters.AddWithValue("@Id", ids[i]);
                        command.ExecuteNonQuery();
                    }
                }
              

                using (SqlCommand command = new SqlCommand("SELECT Name, Age FROM Minions", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} {reader[1]}");
                        }
                    }
                }
            }
        }
    }
}
