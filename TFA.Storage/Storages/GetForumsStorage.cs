using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages;

public class GetForumsStorage(ForumDbContext dbContext) : IGetForumsStorage
{
    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) =>
        await dbContext.Forums.Select(f => new Domain.Models.Forum
        {
            Id = f.ForumId,
            Title = f.Title
        }).ToArrayAsync(cancellationToken);
}