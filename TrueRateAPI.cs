using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;

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
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "options")] HttpRequestData req,
            FunctionContext context)
        {
            var response = req.CreateResponse();

            // ✅ Add CORS headers
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Headers", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

            // ✅ Handle preflight
            if (req.Method == "OPTIONS")
            {
                response.StatusCode = HttpStatusCode.NoContent;
                return response;
            }

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = System.Text.Json.JsonSerializer.Deserialize<UserInput>(requestBody);


                if (data == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteStringAsync("Missing or invalid input.");
                    return response;
                }

                string conn = "Server=tcp:trueratedata.database.windows.net,1433;" + "Database=TrueRateSQLData;" + "User Id=TrueRateData;" +          // or User ID
                              "Password=!FutureFortune500!;" + "Encrypt=True;" + "TrustServerCertificate=False;" +  "Connection Timeout=30;";

                List<Dictionary<string, object>> hospital_data = DatabaseHelper.GetTableData(conn, "hospital_data");

                string json = JsonConvert.SerializeObject(hospital_data);

                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(json);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function crashed: {ex.Message}");
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync("Server error: " + ex.Message);
                return response;
            }
        }
    }
}
