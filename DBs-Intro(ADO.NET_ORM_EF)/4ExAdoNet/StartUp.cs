using System;
using System.Data.SqlClient;

namespace _4ExAdoNet
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
                    string input = Console.ReadLine();
                    string[] inputArr = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    string input2 = Console.ReadLine();
                    string[] inputArr2 = input2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    string townName = inputArr[inputArr.Length - 1];
                    string villanName = inputArr2[inputArr2.Length - 1];
                    string minnionNmae = inputArr[1];
                    int townId = 0;
                    int villanId = 0;
                    int minnionId = 0;

                    string townCommandString = @"SELECT Id FROM Towns WHERE Name = @townName";

                    using (SqlCommand mainCommand = new SqlCommand(townCommandString, connection))
                    {
                        mainCommand.Parameters.AddWithValue("@townName", townName);
                       
                        if (mainCommand.ExecuteScalar() == null)
                        {
                            using (SqlCommand command = new SqlCommand($"INSERT INTO Towns (Name) VALUES (@townName)", connection))
                            {
                                command.Parameters.AddWithValue("@townName", townName);
                                command.ExecuteNonQuery();
                                Console.WriteLine($"Town {townName} was added to the database.");
                            }
                        }
                        townId = (int)mainCommand.ExecuteScalar();
                    }

                    using (SqlCommand sqlCommand =
                        new SqlCommand(@"SELECT Id FROM Villains WHERE Name = @Name", connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", villanName);

                        if (sqlCommand.ExecuteScalar() == null)
                        {
                            using (SqlCommand command =
                                new SqlCommand(
                                    $"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)",
                                    connection))
                            {
                                command.Parameters.AddWithValue("@villainName", villanName);
                                command.ExecuteNonQuery();
                                Console.WriteLine($"Villain {villanName} was added to the database.");
                            }
                        }
                        villanId = (int) sqlCommand.ExecuteScalar();
                    }

                    using (SqlCommand sqlCommand = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @Name", connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", inputArr[1]);
                    
                        if (sqlCommand.ExecuteScalar() == null)
                        {
                            using (SqlCommand command = 
                                new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)", connection))
                            {
                                command.Parameters.AddWithValue("@nam", inputArr[1]);
                                command.Parameters.AddWithValue("@age", int.Parse(inputArr[2]));
                                command.Parameters.AddWithValue("@townId", townId);
                                command.ExecuteNonQuery();                                
                            }                           
                        }
                        minnionId = (int)sqlCommand.ExecuteScalar();

                        using (SqlCommand command = 
                            new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)", connection))
                        {
                            command.Parameters.AddWithValue("@villainId", villanId);
                            command.Parameters.AddWithValue("@minionId", minnionId);
                            try
                            {
                                command.ExecuteNonQuery();
                                Console.WriteLine($"Successfully added {inputArr[1]} to be minion of {villanName}.");
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
    }
}
