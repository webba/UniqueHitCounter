using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Api.StorageService
{
    public class StorageServiceOptions
    {
        public const string StorageServiceOptionsLocation = "StorageServiceOptions";
        public string? ConnectionString { get; set; }
        public string? OldDataName { get; set; }
        public string? FullDataName { get; set; }
        public string? ResultsDataName { get; set; }
    }
}
