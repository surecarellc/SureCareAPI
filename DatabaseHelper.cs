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

    public static List<Dictionary<string, object>> GetSpecificHospitals(List<Dictionary<string, object>> curHospitals, double lat, double lng, int rad)
    {
        List<Dictionary<string, object>> ret = new List<Dictionary<string, object>>();

        foreach (Dictionary<String, object> hospital in curHospitals)
        {
            if (IsHospitalInRange(hospital, lat, lng, rad))
            {
                ret.Add(hospital);
            }
        }

        return ret;
    }

    private static Boolean IsHospitalInRange(Dictionary<string, object> hospital, double lat, double lng, int rad)
    {
        double hospitalLat = (double)hospital["lat"];
        double hospitalLng = (double)hospital["lng"];

        // Calculate distance using Haversine formula
        double earthRadius = 3958.8; // Radius of Earth in miles

        double dLat = DegreesToRadians(hospitalLat - lat);
        double dLng = DegreesToRadians(hospitalLng - lng);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                Math.Cos(DegreesToRadians(lat)) *
                Math.Cos(DegreesToRadians(hospitalLat)) *
                Math.Pow(Math.Sin(dLng / 2), 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = earthRadius * c;

        return distance <= rad;
    }

    // Helper method to convert degrees to radians
    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}