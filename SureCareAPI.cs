using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonSerializer.Deserialize<UserInput>(requestBody);
                //log.LogInformation($"Request body: {requestBody}");
                //|| string.IsNullOrEmpty(data.Name)
                if (data == null )
                {
                    return new BadRequestObjectResult("Missing or invalid 'name' in JSON.");
                }

                return new OkObjectResult($"Hello, {data.Name}! You are {data.Age} years old.");
            }
            catch (Exception ex)
            {
                log.LogError($"Function crashed: {ex.Message}");
                return new ObjectResult($"Server error: {ex.Message}") { StatusCode = 500 };
            }
        }

    }
}