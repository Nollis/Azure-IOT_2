using System;
using System.Runtime.ExceptionServices;
using Azure.Messaging.EventHubs;
using AzureFunctions.Contexts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Functions
{
    public class SaveData
    {
        private readonly ILogger _logger;
        private readonly DataContext _context;

        public SaveData(ILoggerFactory loggerFactory, DataContext context)
        {
            _logger = loggerFactory.CreateLogger<SaveData>();
        }

        [Function("SaveData")]
        public async Task Run([EventHubTrigger("iothub-ehub-nollis1-io-23588002-c7d649ded5", Connection = "IotHubEndpoint", ConsumerGroup = "cosmosdb")] string[] messages, FunctionContext context)
        {
            for ( int i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                _logger.LogInformation($"message: {message}");

                //var enqueuedTimeUtcArray = context.BindingContext.BindingData["enqueuedTimeUtcArray"];
                //_logger.LogInformation($"time: {enqueuedTimeUtcArray}");

                var systemPropertiesArray = context.BindingContext.BindingData["systemPropertiesArray"]?.ToString();
                _logger.LogInformation($"systemproperties: {JsonConvert.SerializeObject(systemPropertiesArray)}");

                _context.Messages.Add(new Models.SaveMessage
                {
                    Message = message,
                    SystemProperties = systemPropertiesArray!
                });
                await _context.SaveChangesAsync();
            }

        }
    }
}
