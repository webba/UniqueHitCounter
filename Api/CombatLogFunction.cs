using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;
using UniqueHitCounter.Logic;
using System.Collections.Generic;
using Newtonsoft.Json;
using BlazorApp.Shared.Entry;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace BlazorApp.Api
{
    public class CombatLogFunction
    {
        private readonly StorageService.StorageService _storageService;
        private readonly ILogger<CombatLogFunction> _logger;

        public CombatLogFunction(StorageService.StorageService storageService, ILogger<CombatLogFunction> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [Function("CombatLog")]
        public async Task<HttpResponseData> PostCombatLog(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CombatLog")] HttpRequestData req)
        {

            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                var body = await req.ReadAsStringAsync();
                var combatPost = JsonConvert.DeserializeObject<CombatPost>(body);
                var logParser = new LogParser(combatPost);
                var entries = logParser.ParseCombatLog();

                var blobName = Guid.NewGuid().ToString();
                CombatResults combatResults = new CombatResults
                {
                    Created = DateTime.Now,
                    Creator = combatPost.Name,
                    FightStart = entries.Min(e => e.LogTime),
                    FightEnd = entries.Max(e => e.LogTime),
                    CombatResultsDict = logParser.ProcessResults(entries)
                };

                await _storageService.SaveFullData(blobName, entries);
                await _storageService.SaveResultsData(blobName, combatResults);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(blobName);

                return response;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [Function("GetCombatLogsFunction")]
        public async Task<HttpResponseData> GetCombatLogs(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CombatLog/{blobName}")] HttpRequestData req,
            string blobName)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var combatEntries = await _storageService.GetFullData(blobName);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(combatEntries);

            return response;
        }

        [Function("GetCombatResultsFunction")]
        public async Task<HttpResponseData> GetCombatResults(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CombatResults/{blobName}")] HttpRequestData req,
          string blobName)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var results = await _storageService.GetResultsData(blobName);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(results);

            return response;
        }

        [Function("GetOldDataFunction")]
        public async Task<HttpResponseData> GetOldData(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OldData/{blobName}")] HttpRequestData req,
          string blobName)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var data = await _storageService.GetOldData(blobName);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}
