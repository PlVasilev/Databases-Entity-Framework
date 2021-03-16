using System;
using System.Data;
using System.Data.SqlClient;

namespace _3ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
                using (connection)
                {
                    string vilanNameCommand = @"SELECT Name FROM Villains WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(vilanNameCommand, connection))
                    {
                        int id = int.Parse(Console.ReadLine());
                        command.Parameters.AddWithValue("@Id", id);

                        string villanName = (string) command.ExecuteScalar();

                        if (villanName == null)
                        {
                            Console.Write($"No villain with ID {id} exists in the database.");
                        }

                        Console.WriteLine($"Villain: {villanName}");
                    }


                    string vilanMinnionsQuerry = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = 1--@Id
                                ORDER BY m.Name";


                    using (SqlCommand command = new SqlCommand(vilanMinnionsQuerry, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("(no minions)");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{(long) reader[0]}. {(string) reader[1]} {(int) reader[2]}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
