using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;
using System.Text.Json;

namespace BlazorApp.Api
{
    public static class CombatLogFunction
    {
        [FunctionName("CombatLogFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("%BlobName%/%FullDataName%", FileAccess.Read)] Stream fullDataBlob,
            [Blob("%BlobName%/%ResultsDataName%", FileAccess.Read)] Stream resultsDataBlob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string body = await req.ReadAsStringAsync();
            var combatPost = JsonSerializer.Deserialize<CombatPost>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });



            return new OkObjectResult("");
        }
    }
}
