namespace Play.Common.Settings;

public class OutboxSettings
{
    public bool Enabled { get; set; }

    public bool PublishOnCommit { get; set; }

    public int IntervalMilliseconds { get; set; } = 2_000;

    public int BatchSize { get; set; } = 1;
}