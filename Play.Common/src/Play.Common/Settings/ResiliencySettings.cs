namespace Play.Common.Settings;

public class ResiliencySettings
{
    public ConsumerResiliencySettings Consumer { get; private set; }
    public ProducerResiliencySettings Producer { get; private set; }
    
    public ResiliencySettings(ConsumerResiliencySettings consumer, ProducerResiliencySettings producer)
    {
        Consumer = consumer;
        Producer = producer;
    }
}

public record ConsumerResiliencySettings(
    bool BrokerRetriesEnabled,
    int BrokerRetriesLimit,
    int ConsumerRetriesLimit);

public record ProducerResiliencySettings(
    bool PublishMandatoryEnabled,
    bool PublisherConfirmsEnabled);    