﻿using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Configuration;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HostedServices
{
    public class StockReplenishedConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockReplenishedConsumerHostedService> _logger;

        protected string Topic { get; set; } = "stock_replenished_event";

        public StockReplenishedConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<StockReplenishedConsumerHostedService> logger)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
            };

            using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                c.Subscribe(Topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            try
                            {
                                await Task.Yield();
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                var cr = c.Consume(stoppingToken);
                                if (cr != null)
                                {
                                    var message = JsonSerializer.Deserialize<StockReplenishedEvent>(cr.Message.Value);
                                    await mediator.Send(new NewMerchandiseAppearedCommand()
                                    {
                                        Items = message.Type
                                    }, stoppingToken);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Error while get consume. Message {ex.Message}");
                            }
                        }
                    }
                }
                finally
                {
                    c.Commit();
                    c.Close();
                }
            }
        }
    }
}