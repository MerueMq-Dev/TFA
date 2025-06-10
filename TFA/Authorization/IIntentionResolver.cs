using TFA.Domain.Identity;

namespace TFA.Domain.Authorization;

public interface IIntentionResolver;

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    public bool IsAllowed(IIdentity subject, TIntention intention);
}
