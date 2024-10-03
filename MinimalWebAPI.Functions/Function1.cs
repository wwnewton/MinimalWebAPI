using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MinimalWebAPI.Functions
{
    public class Function1
    {
        private readonly ILogger<Function1> logger;

        public Function1(ILogger<Function1> logger)
        {
            this.logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run(
            [ServiceBusTrigger("test", Connection = "messaging")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            this.logger.LogInformation("Message ID: {id}", message.MessageId);
            this.logger.LogInformation("Message Body: {body}", message.Body);
            this.logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
