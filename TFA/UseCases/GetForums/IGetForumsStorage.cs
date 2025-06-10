using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

public interface IGetForumsStorage
{
    public Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
}