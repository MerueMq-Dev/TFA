using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.Identity;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateTopic;
// using TFA.Storage;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase _sut;
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> _createTopicSetup;
    private readonly ISetup<ICreateTopicStorage, Task<bool>> _forumExistsSetup;
    private readonly Mock<ICreateTopicStorage> _storage;
    private readonly ISetup<IIdentity, Guid> _getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> _intentionManager;

    public CreateTopicUseCaseShould()
    {
        _storage = new Mock<ICreateTopicStorage>();
        _createTopicSetup = _storage.Setup(f => f.CreateTopic(
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        _forumExistsSetup = _storage.Setup(f => f.ForumExists(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>())
        );

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(i => i.Current).Returns(identity.Object);
        _getCurrentUserIdSetup = identity.Setup(i => i.UserId);
        
        _intentionManager = new Mock<IIntentionManager>();
        _intentionIsAllowedSetup = _intentionManager
             .Setup(i => i.IsAllowed(It.IsAny<TopicIntention>()));
        _sut = new CreateTopicUseCase(_storage.Object, identityProvider.Object, _intentionManager.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenCreationTopicIsNotAllowed()
    {
        var forumId = Guid.Parse("E6D885FB-5C95-428A-8B26-09B05D8A5D5D");
        _intentionIsAllowedSetup.Returns(false);
        await _sut.Invoking(s => s.Execute(forumId, "Title", CancellationToken.None)).Should()
            .ThrowAsync<IntentionManagerException>();
        _intentionManager.Verify(m =>m.IsAllowed(TopicIntention.Create));
    }
    
    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        _intentionIsAllowedSetup.Returns(true);
        _forumExistsSetup.ReturnsAsync(false);
        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");
        await _sut.Invoking(s => s.Execute(forumId, "Some Title", CancellationToken.None)).Should()
            .ThrowAsync<ForumNotFoundException>();
        _storage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
    }


    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("AB1C9640-461D-4915-84B1-0994CE554770");
        var userId = Guid.Parse("BF4F1238-A51A-4C1B-8202-2A1DCC4A2091");
        _forumExistsSetup.ReturnsAsync(true);
        _intentionIsAllowedSetup.Returns(true);
        _getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic()
        {
            Title = "Hello World",
        };
        _createTopicSetup.ReturnsAsync(expected);
        
        var actual = await _sut.Execute(forumId,"Hello World", CancellationToken.None);
        actual.Should().BeEquivalentTo(expected);
        _storage.Verify(t => t.CreateTopic(forumId, userId, "Hello World",
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}