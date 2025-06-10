using TFA.Domain.Authorization;
using TFA.Domain.Identity;

namespace TFA.Domain.UseCases.CreateTopic;

public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthorized(),
        _ => false
    };
}