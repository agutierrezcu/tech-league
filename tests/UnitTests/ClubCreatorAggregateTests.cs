using Domain.Clubs;
using Domain.Clubs.Add;
using Domain.DomainEvents;
using FluentAssertions;
using SharedKernel;

namespace UnitTests;

public sealed class ClubCreatorAggregateTests
{
    [Fact]
    public void CreateClub_ReturnsFailure_WhenBudgetBelowMinimum()
    {
        // Arrange
        Club root = new()
        {
            Name = "Test Club",
            ThreeLettersName = "TST",
            AnualBudget = Club.MinimumAnualBudget
        };

        ClubCreatorAggregate aggregate = new(root);

        // Act
        Result result = aggregate.CreateClub(Club.MinimumAnualBudget - 1m);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ClubCreatorAggregateErrors.MinBudgetNotReached);
    }

    [Fact]
    public void CreateClub_Succeeds_AndRegistersEvent()
    {
        // Arrange
        Club root = new()
        {
            Name = "Test Club",
            ThreeLettersName = "TST",
            AnualBudget = 0m
        };

        ClubCreatorAggregate aggregate = new(root);

        decimal budget = Club.MinimumAnualBudget + 500000m;

        // Act
        Result result = aggregate.CreateClub(budget);

        // Assert
        result.IsSuccess.Should().BeTrue();
        root.AnualBudget.Should().Be(budget);

        IDomainEvent[] events = [..aggregate.GetEvents()];

        events.Length.Should().Be(1);
        var domainEvent = events[0] as NewClubCreatedDomainEvent;
        domainEvent.Should().NotBeNull();
        domainEvent!.ClubId.Should().Be(root.Id);
    }
}
