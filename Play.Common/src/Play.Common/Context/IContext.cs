﻿namespace Play.Common.Context;

public interface IContext
{
    string RequestId { get; }
    string TraceId { get; }
    IIdentityContext IdentityContext { get; }
}