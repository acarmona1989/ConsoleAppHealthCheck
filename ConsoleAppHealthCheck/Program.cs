using ConsoleAppHealthCheck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args)
                    .Build();
        try
        {
            await host.RunAsync();
            Environment.Exit(0);
        }
        catch (Exception)
        {

            Environment.Exit(1);
        }
        
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services
            .AddHealthChecks()
            .AddCheck("healthcheck", () => HealthCheckResult.Healthy());

            services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(5);
                options.Period = TimeSpan.FromSeconds(20);
            });

            services.AddHostedService<MyService>();
        });
}