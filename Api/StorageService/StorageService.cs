using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorApp.Shared;
using BlazorApp.Shared.Entry;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Api.StorageService
{
    public class StorageService
    {
        private readonly BlobContainerClient _fullDataContainer;

        private readonly BlobContainerClient _oldDataContainer;

        private readonly BlobContainerClient _resultsDataContainer;

        public StorageService(IOptions<StorageServiceOptions> options)
        {
            if(options.Value == null 
                || options.Value.ConnectionString == null 
                || options.Value.FullDataName == null 
                || options.Value.ResultsDataName == null 
                || options.Value.OldDataName == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _fullDataContainer = new BlobContainerClient(options.Value.ConnectionString, options.Value.FullDataName);
            _oldDataContainer = new BlobContainerClient(options.Value.ConnectionString, options.Value.OldDataName);
            _resultsDataContainer = new BlobContainerClient(options.Value.ConnectionString, options.Value.ResultsDataName);
        }

        public async Task<IEnumerable<LogEntry>> GetFullData(string guid)
        {
            var blob = _fullDataContainer.GetBlobClient(guid);
            if(blob == null || !await blob.ExistsAsync())
            {
                return new List<LogEntry>();
            }
            
            var blobData = await blob.DownloadContentAsync();
            var blobDataString = blobData.Value.Content.ToString();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var entries = JsonConvert.DeserializeObject<IEnumerable<LogEntry>>(blobDataString, settings);

            return entries;
        }

        public async Task SaveFullData(string guid, IEnumerable<LogEntry> entries)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var jsonData = JsonConvert.SerializeObject(entries, settings);

            var blob = _fullDataContainer.GetBlobClient(guid);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = "text/json" };
            await blob.UploadAsync(BinaryData.FromString(jsonData), new BlobUploadOptions { HttpHeaders = blobHttpHeader });
        }

        public async Task<IEnumerable<OldDataEntry>> GetOldData(string guid)
        {
            var blob = _oldDataContainer.GetBlobClient(guid);
            if (blob == null || !await blob.ExistsAsync())
            {
                return new List<OldDataEntry>();
            }

            var blobData = await blob.DownloadContentAsync();
            var blobDataString = blobData.Value.Content.ToString();
            var entries = JsonConvert.DeserializeObject<IEnumerable<OldDataEntry>>(blobDataString);

            return entries;
        }

        public async Task<CombatResults?> GetResultsData(string guid)
        {
            var blob = _resultsDataContainer.GetBlobClient(guid);
            if (blob == null || !await blob.ExistsAsync())
            {
                return null;
            }

            var blobData = await blob.DownloadContentAsync();
            var blobDataString = blobData.Value.Content.ToString();
            var entries = JsonConvert.DeserializeObject<CombatResults>(blobDataString);

            return entries;
        }

        public async Task SaveResultsData(string guid, CombatResults entries)
        {
            var jsonData = JsonConvert.SerializeObject(entries);

            var blob = _resultsDataContainer.GetBlobClient(guid);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = "text/json" };
            await blob.UploadAsync(BinaryData.FromString(jsonData), new BlobUploadOptions { HttpHeaders = blobHttpHeader });
        }
    }
}
