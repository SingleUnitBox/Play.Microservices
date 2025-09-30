namespace Play.Common.Messaging.Executor;

public class MessageExecutionAbortedException(string message) : Exception(message);