using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;


namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly ForumDbContext _forumDbContext;
    private readonly CreateTopicUseCase _sut;
    private readonly ISetup<IGuidFactory, Guid> createIdSetup;
    private readonly ISetup<IMomentProvider, DateTimeOffset> getNowSetup;

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var newForum = new Forum()
            { ForumId = Guid.Parse("C020701B-ECDD-44BB-BEAA-1AC0F369EAA0"), Title = "Тестовый форум" };
        await _forumDbContext.Forums.AddAsync(newForum);
        await _forumDbContext.SaveChangesAsync();

        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");
        var authorId = Guid.Parse("69CB6614-3B92-4E6D-9C43-991B3111A56D");

        await _sut.Invoking(s => s.Execute(forumId, "Some Title", authorId, CancellationToken.None)).Should()
            .ThrowAsync<ForumNotFoundException>();
    }

    public CreateTopicUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
        _forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);
        var guidFactary = new Mock<IGuidFactory>();
        createIdSetup = guidFactary.Setup(f => f.Create());
        
        var momentProvider  = new Mock<IMomentProvider>();
        getNowSetup = momentProvider.Setup(f => f.Now);
        _sut = new CreateTopicUseCase(_forumDbContext,guidFactary.Object,momentProvider.Object);
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("AB1C9640-461D-4915-84B1-0994CE554770");
        var userId = Guid.Parse("BF4F1238-A51A-4C1B-8202-2A1DCC4A2091");
        var testForum = new Forum()
            { ForumId = forumId, Title = "Hello World" };
        var testUser = new User() { UserId = userId, Loign = "Ingrad", };
        await _forumDbContext.Forums.AddAsync(testForum);
        await _forumDbContext.Users.AddAsync(testUser);
        await _forumDbContext.SaveChangesAsync();
        
        createIdSetup.Returns(Guid.Parse("EE9E6BF4-603C-4E10-872A-F04D5159BE1E"));
        getNowSetup.Returns(new DateTimeOffset(2024, 04, 14, 02, 19, 00, TimeSpan.FromHours(3)));
        
        var actual = await _sut.Execute(forumId, "Hello World", userId, CancellationToken.None);

        var allTopics = await _forumDbContext.Topics.ToArrayAsync();
        allTopics.Should().BeEquivalentTo(new[]{
            new Topic
            {
                ForumId = forumId,
                UserId = userId,
                Title = "Hello World",
            }
        }, cfg => cfg.Including(t=> t.ForumId)
            .Including(t=>t.UserId).Including(t=> t.Title));


        actual.Should().BeEquivalentTo(new Models.Topic()
        {
            Id = Guid.Parse("EE9E6BF4-603C-4E10-872A-F04D5159BE1E"),
            CreatedAt = new DateTimeOffset(2024,04,14,02,19,00,TimeSpan.FromHours(3)),
            Aurhor = "Ingrad",
            Title = "Hello World"
        });
    }
}