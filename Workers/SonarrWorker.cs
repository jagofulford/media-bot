using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using media_bot.Models;
using media_bot.Repositories;

namespace media_bot.Workers
{
    class SonarrWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IMediaRepository _sonarrRepository;
        public SonarrWorker(ILogger<Worker> logger)
        {
            _logger = logger;
            _sonarrRepository = new SonarrRepository();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var results = _sonarrRepository.SearchMedia("test");
                foreach (var result in results)
                {
                    _logger.LogInformation("Found result: {displayTitle}", result.Value.GetMediaDisplayTitle());
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
