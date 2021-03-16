using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            int num;

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
                string country = Console.ReadLine();

                string commandText = @"UPDATE Towns
                                       SET Name = UPPER(Name)
                                       WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand command = new SqlCommand(commandText,connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);
                    num = command.ExecuteNonQuery();
                }

                if (num == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(@" SELECT t.Name 
                                                                    FROM Towns as t
                                                                    JOIN Countries AS c ON c.Id = t.CountryCode
                                                                    WHERE c.Name = @countryName", connection))
                    {
                        command.Parameters.AddWithValue("@countryName", country);

                        List<string> resuList = new List<string>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resuList.Add((string)reader[0]);
                            }
                        }

                        Console.WriteLine($"{resuList.Count} town names were affected. [{string.Join(", ",resuList)}]");
                    }
                }
                
            }
        }
    }
}
