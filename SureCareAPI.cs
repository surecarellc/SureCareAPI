using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

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
            return new OkObjectResult("Function started");

            log.LogInformation("C# HTTP POST trigger function received a request.");

            // Read and deserialize JSON body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<UserInput>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return new BadRequestObjectResult("Please provide a valid JSON body with 'name' and 'age'.");
            }

            return new OkObjectResult($"Hello, {data.Name}! You are {data.Age} years old.");
        }

    }
}