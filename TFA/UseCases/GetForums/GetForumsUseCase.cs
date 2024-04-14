using Microsoft.EntityFrameworkCore;
using TFA.Storage;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly ForumDbContext _forumDbForumDbContext;

    public GetForumsUseCase(ForumDbContext forumDbContext)
    {
        _forumDbForumDbContext = forumDbContext;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) =>
        await _forumDbForumDbContext.Forums
            .Select(f => new Forum
            {
                Id = f.ForumId,
                Title = f.Title
            })
            .ToListAsync(cancellationToken);
}