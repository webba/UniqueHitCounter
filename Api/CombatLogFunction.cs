using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;
using UniqueHitCounter.Logic;
using System.Collections.Generic;
using Newtonsoft.Json;
using BlazorApp.Shared.Entry;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace BlazorApp.Api
{
    public static class CombatLogFunction
    {
        [FunctionName("CombatLogFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CombatLog")] HttpRequest req,
            [Blob("%FullDataName%", FileAccess.Write, Connection = "BlobConnectionString")] BlobContainerClient fullDataBlob,
            [Blob("%ResultsDataName%", FileAccess.Write, Connection = "BlobConnectionString")] BlobContainerClient resultsDataBlob,
            ILogger log)
        {

            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                string body = await req.ReadAsStringAsync();
                var combatPost = JsonConvert.DeserializeObject<CombatPost>(body);
                LogParser logParser = new LogParser(combatPost);
                IEnumerable<LogEntry> result = logParser.ParseCombatLog();

                var blobName = Guid.NewGuid().ToString();
                CombatResults combatResults = new CombatResults
                {
                    Created = DateTime.Now,
                    Creator = combatPost.Name,
                    FightStart = result.Min(e => e.LogTime),
                    FightEnd = result.Max(e => e.LogTime),
                    CombatResultsDict = logParser.ProcessResults(result)
                };

                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

                string result1 = JsonConvert.SerializeObject(result, settings);
                await SaveFile(fullDataBlob, result1, blobName);
                await SaveFile(resultsDataBlob, JsonConvert.SerializeObject(combatResults, settings), blobName);

                return new OkObjectResult(blobName);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private static async Task SaveFile(BlobContainerClient fullDataBlob, string result, string blobName)
        {
            var fullBlockBlob = fullDataBlob.GetBlobClient(blobName);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = "text/json" };
            await fullBlockBlob.UploadAsync(BinaryData.FromString(result),  new BlobUploadOptions { HttpHeaders = blobHttpHeader });
        }

        [FunctionName("CombatLogGetFunction")]
        public static async Task<IActionResult> CombatLogGetFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "CombatLog/{blobName}")] HttpRequest req,
            [Blob("%FullDataName%", FileAccess.Read, Connection = "BlobConnectionString")] BlobContainerClient fullDataBlob,
            ILogger log, string blobName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fullBlockBlob = fullDataBlob.GetBlobClient(blobName);
            var response = await fullBlockBlob.DownloadContentAsync();
            string v = response.Value.Content.ToString();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            IEnumerable<LogEntry> combatEntry = JsonConvert.DeserializeObject<IEnumerable<LogEntry>>(v, settings);

            return new OkObjectResult(combatEntry);
        }

        [FunctionName("CombatResultsGetFunction")]
        public static async Task<IActionResult> CombatResultsGetFunction(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "CombatResults/{blobName}")] HttpRequest req,
          [Blob("%ResultsDataName%", FileAccess.Read, Connection = "BlobConnectionString")] BlobContainerClient fullDataBlob,
          ILogger log, string blobName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fullBlockBlob = fullDataBlob.GetBlobClient(blobName);
            var response = await fullBlockBlob.DownloadContentAsync();
            string v = response.Value.Content.ToString();

            CombatResults combatEntry = JsonConvert.DeserializeObject<CombatResults>(v);

            return new OkObjectResult(combatEntry);
        }

        [FunctionName("OldDataGetFunction")]
        public static async Task<IActionResult> OldDataGetFunction(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "OldData/{blobName}")] HttpRequest req,
          [Blob("%OldDataName%", FileAccess.Read, Connection = "BlobConnectionString")] BlobContainerClient fullDataBlob,
          ILogger log, string blobName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fullBlockBlob = fullDataBlob.GetBlobClient(blobName);
            var response = await fullBlockBlob.DownloadContentAsync();
            string v = response.Value.Content.ToString();

            IEnumerable<OldDataEntry> data = JsonConvert.DeserializeObject<IEnumerable<OldDataEntry>>(v);

            return new OkObjectResult(data);
        }
    }
}
