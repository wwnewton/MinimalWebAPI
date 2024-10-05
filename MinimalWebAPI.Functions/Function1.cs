namespace MinimalWebAPI.Functions;

using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MassTransit;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

/// <summary>
/// Function one.
/// </summary>
public class Function1
{
    private readonly ILogger<Function1> logger;
    private readonly IMessageReceiver messageReceiver;

    /// <summary>
    /// Initializes a new instance of the <see cref="Function1"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="messageReceiver">Message receiver.</param>
    public Function1(ILogger<Function1> logger, IMessageReceiver messageReceiver)
    {
        this.messageReceiver = messageReceiver;
        this.logger = logger;
    }

    /// <summary>
    /// Triggers on service bus message.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="messageActions">Actions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Function(nameof(Function1))]
    public async Task Run(
        [ServiceBusTrigger("test", Connection = "messaging")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        await this.messageReceiver.HandleConsumer<NoteConsumer>("test", message, cancellationToken);
        //this.logger.LogInformation("Message ID: {id}", message.MessageId);
        //this.logger.LogInformation("Message Body: {body}", message.Body);
        //this.logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        //// Complete the message
        //await messageActions.CompleteMessageAsync(message);
    }
}
