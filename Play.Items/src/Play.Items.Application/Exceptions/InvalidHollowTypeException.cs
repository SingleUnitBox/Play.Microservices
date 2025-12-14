using Play.Common.Abs.Exceptions;
using Play.Items.Domain.Types;

namespace Play.Items.Application.Exceptions;

public class InvalidHollowTypeException : PlayException
{
    public string HollowType { get; }
    
    public InvalidHollowTypeException(string hollowType) : base($"Hollow type of '{hollowType}' for socket is invalid. " +
                                                                $"Allowed types are: {FormatAllowedTypes()}.")
    {
        HollowType = hollowType;
    }
    
    private static string FormatAllowedTypes()
        => string.Join("' ", HollowHelper.GetHollowTypes().Select(t => $"'{t}'"));
}