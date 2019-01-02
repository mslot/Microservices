using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;

namespace WebAPI
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private IConfigurationRoot _configRoot;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfigurationRoot configRoot)
        {
            _logger = logger;
            _configRoot = configRoot;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateConfigSources, null, TimeSpan.Zero,
                TimeSpan.FromHours(5));

            return Task.CompletedTask;
        }

        //Soul purpose for this task is to make sure keyvault secrets get flushed and updated once in a while
        private void UpdateConfigSources(object state)
        {
            _configRoot.Reload();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
