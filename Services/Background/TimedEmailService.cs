using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotesOTG_Server.Services.Background
{
    public class TimedEmailService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedEmailService> logger;
        private Timer timer;

        public TimedEmailService(ILogger<TimedEmailService> logger)
        {
            this.logger = logger;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Email Service Running");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            logger.LogInformation("Timed Email Servvice is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Time Email Service is stopping");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
