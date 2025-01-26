namespace Play.Common.Abs.Events;

public interface IRejectedEvent : IEvent
{
    string Reason { get; }
    string Code { get; }
}