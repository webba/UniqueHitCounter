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
using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlazorApp.Api
{
    public static class CombatLogFunction
    {
        [FunctionName("CombatLogFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CombatLog")] HttpRequest req,
            [Blob("%FullDataName%", FileAccess.Write)] CloudBlobContainer fullDataBlob,
            [Blob("%ResultsDataName%", FileAccess.Write)] CloudBlobContainer resultsDataBlob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string body = await req.ReadAsStringAsync();
            var combatPost = JsonConvert.DeserializeObject<CombatPost>(body);
            LogParser logParser = new LogParser(combatPost);
            IEnumerable<object> result = logParser.ParseCombatLog();

            var blobName = Guid.NewGuid().ToString();
            CombatResults combatResults = new CombatResults { 
                Created = DateTime.Now,
                Creator = combatPost.Name,
                FightStart = DateTime.Now,
                FightEnd = DateTime.Now,
                CombatResultsList = new List<CombatResult>() 
                { 
                    new CombatResult { 
                        Player = "test",
                        BiggestHit = "test",
                        Hits = 10,
                        KnocksOver = 10,
                        TimesHit = 10,
                        TimesStunned = 10,
                        TimesThrown = 10
                    } 
                } 
            };

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

            string result1 = JsonConvert.SerializeObject(result, settings);
            await SaveFile(fullDataBlob, result1, blobName);
            await SaveFile(resultsDataBlob, JsonConvert.SerializeObject(combatResults, settings), blobName);

            return new OkObjectResult(blobName);
        }

        private static async Task SaveFile(CloudBlobContainer fullDataBlob, string result, string blobName)
        {
            var fullBlockBlob = fullDataBlob.GetBlockBlobReference(blobName);
            fullBlockBlob.Properties.ContentType = "text/json";
            await fullBlockBlob.UploadTextAsync(result);
        }

        [FunctionName("CombatLogGetFunction")]
        public static async Task<IActionResult> CombatLogGetFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "CombatLog/{blobName}")] HttpRequest req,
            [Blob("%FullDataName%", FileAccess.Read)] CloudBlobContainer fullDataBlob,
            ILogger log, string blobName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fullBlockBlob = fullDataBlob.GetBlockBlobReference(blobName);
            string v = await fullBlockBlob.DownloadTextAsync();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            IEnumerable<LogEntry> combatEntry = JsonConvert.DeserializeObject<IEnumerable<LogEntry>>(v, settings);

            return new OkObjectResult(combatEntry);
        }

        [FunctionName("CombatResultsGetFunction")]
        public static async Task<IActionResult> CombatResultsGetFunction(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "CombatResults/{blobName}")] HttpRequest req,
          [Blob("%ResultsDataName%", FileAccess.Read)] CloudBlobContainer fullDataBlob,
          ILogger log, string blobName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fullBlockBlob = fullDataBlob.GetBlockBlobReference(blobName);
            string v = await fullBlockBlob.DownloadTextAsync();

            CombatResults combatEntry = JsonConvert.DeserializeObject<CombatResults>(v);

            return new OkObjectResult(combatEntry);
        }
    }
}
