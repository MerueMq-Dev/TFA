using TFA.Domain.Identity;

namespace TFA.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : Enum;
    bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : Enum;
}

public class IntentionManager(IEnumerable<IIntentionResolver> resolvers, IIdentityProvider identityProvider):IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : Enum
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }

    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : Enum
    {
        throw new NotImplementedException();
    }
}

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : Enum
    {
        if (!intentionManager.IsAllowed(intention))
            throw new IntentionManagerException();
    }
}