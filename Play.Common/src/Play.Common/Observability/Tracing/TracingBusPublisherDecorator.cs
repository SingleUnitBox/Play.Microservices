﻿using System.Diagnostics;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging;

namespace Play.Common.Observability.Tracing;

internal sealed class TracingBusPublisherDecorator(IBusPublisher innerBusPublisher,
    MessagePropertiesAccessor messagePropertiesAccessor) : IBusPublisher
{
    public async Task Publish<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        string routingKey = "",
        ICorrelationContext correlationContext = null,
        IDictionary<string, object> headers = null) where TMessage : class
    {
        var messageProperties = messagePropertiesAccessor.InitializeIfEmpty();
        using var activity = CreateMessagingExecutionActivity(messageProperties, message.GetType());
        
        await innerBusPublisher.Publish(message, exchangeName, messageId, routingKey,
            correlationContext, messageProperties.Headers);
    }

    private Activity? CreateMessagingExecutionActivity(MessageProperties messageProperties, Type messageType)
    {
        var activitySource = new ActivitySource(MessagingActivitySource.MessagingPublishSourceName);
        var activity = activitySource.StartActivity($"Message execution: {messageType.Name}",
            ActivityKind.Producer, Activity.Current?.Context ?? default);
        if (activity is not null)
        {
            messageProperties.Headers.TryAdd(MessagingObservabilityHeaders.TraceParent, activity.Id);
        }
        
        return activity;
    }
}