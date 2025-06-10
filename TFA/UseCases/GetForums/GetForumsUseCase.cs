using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage storage) : IGetForumsUseCase
{
    public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) =>
        await storage.GetForums(cancellationToken);
}