using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase : IGetForumsUseCase
{
    public Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}