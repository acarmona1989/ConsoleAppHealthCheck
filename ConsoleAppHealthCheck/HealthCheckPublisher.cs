using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace ConsoleAppHealthCheck
{
    public class HealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly string _fileName;
        private HealthStatus _prevStatus = HealthStatus.Unhealthy;

        public HealthCheckPublisher()
        {
            _fileName = Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "alive.txt");
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            var fileExists = _prevStatus == HealthStatus.Healthy;

            if (report.Status == HealthStatus.Healthy)
            {
                using var _ = File.Create(_fileName);
            }
            else if (fileExists)
            {
                File.Delete(_fileName);
            }

            _prevStatus = report.Status;

            return Task.CompletedTask;
        }
    }
}
