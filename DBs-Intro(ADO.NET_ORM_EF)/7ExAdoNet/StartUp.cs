using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _7ExAdoNet
{
    class StartUp
    {
        static void Main(string[] args)
        {
            List<string> minionNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWhithDB))
            {
                connection.Open();
               
                using (SqlCommand command = new SqlCommand("SELECT Name FROM Minions",connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minionNames.Add((string)reader[0]);
                        }                      
                    }
                }
            }

            for (int i = 0; i < minionNames.Count/2 + 1; i++)
            {
                Console.WriteLine(minionNames[i]);
                if (minionNames.Count / 2 == i)
                {
                    break;
                }
                Console.WriteLine(minionNames[minionNames.Count - 1 - i]);                
            }
        }
    }
}
