using Play.Common.Settings;

namespace Play.Common.Messaging.Resiliency;

public class ReliablePublishing(ResiliencySettings resiliencySettings)
{
    public bool UsePublisherConfirms => resiliencySettings.Producer.PublisherConfirmsEnabled;

    public bool ShouldPublishAsMandatory()
    {
        if (resiliencySettings.Producer.PublishMandatoryEnabled is false)
        {
            return false;
        }
        
        return true;
    }
}