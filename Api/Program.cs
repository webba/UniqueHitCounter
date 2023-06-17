using BlazorApp.Api.StorageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorApp.Api
{
    public class Program
    {
        public static void Main()
        {
            var builder = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureLogging(o => { });

            builder.ConfigureServices(s =>
                {
                    s.AddOptions<StorageServiceOptions>()
                        .Configure<IConfiguration>((settings, configuration) =>
                        {
                            configuration.GetSection(StorageServiceOptions.StorageServiceOptionsLocation).Bind(settings);
                        });
                    s.AddTransient<StorageService.StorageService>();
                });

            var host = builder.Build();

            host.Run();
        }
    }
}
