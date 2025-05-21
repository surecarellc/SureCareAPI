using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

class DatabaseHelper
{
    public static List<Dictionary<string, object>> GetTableData(string connectionString, string tableName)
    {
        var result = new List<Dictionary<string, object>>();
        string query = $"SELECT * FROM {tableName}";

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int count = 0;

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object value = null;

                            if (reader.IsDBNull(i))
                            {
                                value = null;
                            }
                            else if (columnName == "lat" || columnName == "lng" || columnName == "rating")
                            {
                                value = reader.GetDouble(i);
                            }
                            else
                            {
                                value = reader[i]?.ToString() ?? string.Empty;
                            }

                            row[columnName] = value;
                        }
                        result.Add(row);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add(new Dictionary<string, object>
                {
                    { "key1", ex.Message }
                });
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        return result;
    }
}
