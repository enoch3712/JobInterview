using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hahn.ApplicationProcess.July2021.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AddSerilog();

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        private static void AddSerilog()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
				.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
