using Microsoft.EntityFrameworkCore;
using TFA.Domain.Exceptions;
using TFA.Storage;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase: ICreateTopicUseCase
{
    private readonly ForumDbContext _forumDbForumDbContext;
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;
    public CreateTopicUseCase(ForumDbContext forumDbContext, IGuidFactory guidFactory, IMomentProvider momentProvider)
    {
        
        _forumDbForumDbContext = forumDbContext;
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
    }
    
    public async Task<Topic> Execute(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
    {
        var forumExists = await _forumDbForumDbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        var topicId = _guidFactory.Create();
        await _forumDbForumDbContext.Topics.AddAsync(new Storage.Topic()
        {
            CreatedAt = _momentProvider.Now,
            TopicId = topicId,
            ForumId = forumId,
            Title = title,
            UserId = authorId,
        });
        await _forumDbForumDbContext.SaveChangesAsync();

        return await _forumDbForumDbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new Topic()
            {
                Id = t.TopicId,
                Title = t.Title,
                Aurhor = t.Author.Loign,
                CreatedAt = t.CreatedAt,
            }).FirstAsync();
    }
}