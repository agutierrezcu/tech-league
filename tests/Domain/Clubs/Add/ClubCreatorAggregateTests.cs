using System.Linq;
using Shouldly;
using Xunit;
using Domain.Clubs;
using Domain.Clubs.Add;
using Domain.DomainEvents;
using SharedKernel;

namespace Domain.Clubs.Add.Tests;

public sealed class ClubCreatorAggregateTests
{
    [Fact]
    public void CreateClub_ReturnsFailure_WhenBudgetBelowMinimum()
    {
        // Arrange
        Club root = new Club
        {
            Name = "Test Club",
            ThreeLettersName = "TST",
            AnualBudget = Club.MinimumAnualBudget
        };

        ClubCreatorAggregate aggregate = new ClubCreatorAggregate(root);

        // Act
        Result result = aggregate.CreateClub(Club.MinimumAnualBudget - 1m);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ClubCreatorAggregateErrors.MinBudgetNotReached);
    }

    [Fact]
    public void CreateClub_Succeeds_AndRegistersEvent()
    {
        // Arrange
        Club root = new Club
        {
            Name = "Test Club",
            ThreeLettersName = "TST",
            AnualBudget = 0m
        };

        ClubCreatorAggregate aggregate = new ClubCreatorAggregate(root);

        decimal budget = Club.MinimumAnualBudget + 500000m;

        // Act
        Result result = aggregate.CreateClub(budget);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        root.AnualBudget.ShouldBe(budget);

        System.Collections.Generic.IEnumerable<IDomainEvent> events = aggregate.GetEvents();
        IDomainEvent[] eventsArray = events.ToArray();

        eventsArray.Length.ShouldBe(1);
        NewClubCreatedDomainEvent? domainEvent = eventsArray[0] as NewClubCreatedDomainEvent;
        domainEvent.ShouldNotBeNull();
        domainEvent!.ClubId.ShouldBe(root.Id);
    }
}
