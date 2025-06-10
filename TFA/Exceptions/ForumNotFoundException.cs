namespace TFA.Domain.Exceptions;

public class ForumNotFoundException(Guid forumId) : Exception($"Forum with id {forumId} was not found");