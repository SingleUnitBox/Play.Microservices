namespace Play.Operation.Api.Infrastructure;

public static class Extensions
{
    public static string ToUserGroup(this Guid userId) => userId.ToString("N").ToUserGroup();
    public static string ToUserGroup(this string userId) => $"users:{userId}";
}