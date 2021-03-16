using System;
using System.Data.SqlClient;

namespace _6ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();

                int releasedMinnions = 0;
                string villanName = "";
                int villanId = int.Parse(Console.ReadLine());

                using (SqlCommand command = new SqlCommand("SELECT Name FROM Villains WHERE Id = @villainId", connection))
                {
                    command.Parameters.AddWithValue("@villainId", villanId);
                    villanName = (string)command.ExecuteScalar();
                }

                if (villanName == null)
                {
                    Console.WriteLine("No such villain was found.");
                }
                else
                {

                    try
                    {
                        using (SqlCommand command = new SqlCommand("DELETE FROM MinionsVillains WHERE VillainId = @villainId", connection))
                        {
                            command.Parameters.AddWithValue("@villainId", villanId);
                            releasedMinnions = command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand("DELETE FROM Villains WHERE Id = @villainId", connection))
                        {
                            command.Parameters.AddWithValue("@villainId", villanId);
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"{villanName} was deleted.\n{releasedMinnions} minions were released.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                      
                    }
                }
            }
        }
    }
}
