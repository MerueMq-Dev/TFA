using Microsoft.EntityFrameworkCore;
using TFA.Domain;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Storage.Storages;

public class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    ForumDbContext dbContext) : ICreateTopicStorage
{
    public async Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken) =>
        await dbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);

    public async Task<Domain.Models.Topic> CreateTopic(Guid forumId, Guid userId, string title,
        CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var newTopic = new Topic()
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = userId,
            Title = title,
            CreatedAt = momentProvider.Now
        };

        await dbContext.Topics.AddAsync(newTopic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return dbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new Domain.Models.Topic
            {
               Id = t.TopicId,
               ForumId = t.ForumId,
               Title = t.Title,
               UserId = t.UserId,
               CreatedAt = t.CreatedAt
            })
            .First();
    }
}