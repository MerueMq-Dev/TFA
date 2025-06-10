namespace TFA.Domain.Identity;

public interface IIdentityProvider
{
    public IIdentity Current { get; }
}

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; } = new User(Guid.Parse("CDC858A3-DB5B-4A76-957E-72E6B1CBFE78"));
}