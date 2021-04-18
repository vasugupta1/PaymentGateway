using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PaymentGateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuartion) => 
                {
                    configuartion.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                        new Uri(
                            context.Configuration["CustomConfiguration:ElasticConfiguartion:ConnectionString"]))
                        {
                            IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-dev-{DateTime.UtcNow:yyyyy-MM}",
                            AutoRegisterTemplate = true,
                            NumberOfShards =2,
                            NumberOfReplicas = 1
                        })
                    .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
