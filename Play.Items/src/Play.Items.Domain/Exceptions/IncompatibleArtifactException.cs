using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class IncompatibleArtifactException : PlayException
{
    public IncompatibleArtifactException(string hollowType, string artifact)
        : base($"'{hollowType}' socket cannot be occupied by '{artifact}' artifact.")
    {
    }
}