namespace TFA.Domain.Identity;

public interface IIdentity
{
    Guid UserId { get; }
}

public class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;
}
    

public static class IdentityExtensions
{
    public static bool IsAuthorized(this IIdentity identity) => identity.UserId != Guid.Empty;
}