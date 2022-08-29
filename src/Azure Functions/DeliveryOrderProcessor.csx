#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static IActionResult Run(HttpRequest req, out object outputDocument, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string requestBody = new StreamReader(req.Body).ReadToEnd();
    try
    {
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        if (!string.IsNullOrEmpty(requestBody))
        {
            log.LogInformation($"Request contains payload: {requestBody}, posting to Cosmos DB");
            outputDocument = new
            {
                id = System.Guid.NewGuid().ToString(),
                order = data
            };
        }
        else
        {
            log.LogInformation("Request doesn't contain payload, exiting");
            outputDocument = null;
            return new BadRequestObjectResult("Empty request");
        }
    }
    catch (Exception ex)
    {
        log.LogInformation($"Exception occured: {ex.Message}");
        outputDocument = null;
        return new BadRequestObjectResult("Failed to process request");
    }

    return new OkObjectResult("Order successfully added");
}