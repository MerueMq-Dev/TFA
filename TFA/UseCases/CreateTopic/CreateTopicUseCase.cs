using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.Identity;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(ICreateTopicStorage storage,
    IIdentityProvider identityProvider, 
    IIntentionManager intentionManager)
    : ICreateTopicUseCase
{
    public async Task<Topic> Execute(Guid forumId, string title, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        bool forumExists = await storage.ForumExists(forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }
        return await storage.CreateTopic(forumId,identityProvider.Current.UserId,title,cancellationToken);
    }
}