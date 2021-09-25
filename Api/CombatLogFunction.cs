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
using UniqueHitCounter.Logic;
using Microsoft.Azure.Storage.Blob;

namespace BlazorApp.Api
{
    public static class CombatLogFunction
    {
        [FunctionName("CombatLogFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("%FullDataName%", FileAccess.Write)] CloudBlobContainer fullDataBlob,
            [Blob("%ResultsDataName%", FileAccess.Write)] CloudBlobContainer resultsDataBlob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string body = await req.ReadAsStringAsync();
            var combatPost = JsonSerializer.Deserialize<CombatPost>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });

            var result = LogParser.ParseCombatLog(combatPost);

            var blobName = Guid.NewGuid().ToString();
            var fullBlockBlob = fullDataBlob.GetBlockBlobReference(blobName);
            fullBlockBlob.Properties.ContentType = "application/json";
            await fullBlockBlob.UploadTextAsync(JsonSerializer.Serialize(result));

            return new OkObjectResult(blobName);
        }
    }
}
