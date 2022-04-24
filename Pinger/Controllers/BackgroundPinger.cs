using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    public class BackgroundPinger : IHostedService
    {
        private readonly ILogger<BackgroundPinger> _logger;
        private Timer _timer;
        public BackgroundPinger(ILogger<BackgroundPinger> logger)
        {
            this._logger = logger;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer=new Timer(o=>_logger.LogInformation("ping"),null,TimeSpan.Zero,TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
