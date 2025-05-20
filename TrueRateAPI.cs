using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Company.Function
{
    public class HttpTriggerCSharp
    {
        private readonly ILogger<HttpTriggerCSharp> _logger;

        public HttpTriggerCSharp(ILogger<HttpTriggerCSharp> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerCSharp")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = System.Text.Json.JsonSerializer.Deserialize<UserInput>(requestBody);


                if (data == null )
                {
                    return new BadRequestObjectResult("Missing or invalid 'name' in JSON.");
                }

                String conn = "DRIVER={ODBC Driver 17 for SQL Server};SERVER=trueratedata.database.windows.net;DATABASE=TrueRateSQLData;UID=TrueRateData;PWD=!FutureFortune500!";

                //List<Dictionary<String, Object>> hospital_data = DatabaseHelper.GetTableData(conn, "hospital_data");
                
                var hospital_data = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "name", "Hospital A" },
                        { "lat", 32.78f },
                        { "lng", -96.8f }
                    },
                    new Dictionary<string, object>
                    {
                        { "name", "Hospital B" },
                        { "lat", 34.05f },
                        { "lng", -118.25f }
                    }
                };

                string json = JsonConvert.SerializeObject(hospital_data);
                return new ContentResult
                {
                    Content = json,
                    ContentType = "application/json",
                    StatusCode = 200
                };

            }
            catch (Exception ex)
            {
                log.LogError($"Function crashed: {ex.Message}");
                return new ObjectResult($"Server error: {ex.Message}") { StatusCode = 500 };
            }
        }

    }
}