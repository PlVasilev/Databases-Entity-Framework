namespace _2ExAdoNet
{
    using System;
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
                using (connection)
                {
                    string commandString = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount
                    FROM Villains AS v
                    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                    GROUP BY v.Id, v.Name
                        HAVING COUNT(mv.VillainId) > 3
                    ORDER BY COUNT(mv.VillainId)";

                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {                      
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string villanName = (string) reader[0];
                                int minionsCount = (int) reader[1];
                                Console.WriteLine($"{villanName} - {minionsCount}");
                            }
                        }
                    }
                }
            }
        }
    }
}
