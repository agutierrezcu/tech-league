using Bogus;
using Domain.Clubs;
using Domain.Players;
using Domain.Players.SignUp;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using SharedKernel;
using static Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace UnitTests;

public class SignUpPlayerAggregateUnitTest
{
    private static readonly Faker<Club> ClubFaker = new();

    private static readonly Faker<Player> PlayerFaker = new();

    [Fact]
    public void Update_ShouldFail_WhenToday_IsGreaterSigningWindow()
    {
        // Arrange
        DateRange signingWindow = DateRange.Create(
            DateTime.Today.AddMonths(-2), DateTime.Today.AddMonths(1)).Value;

        DateTime todayMock = signingWindow.End.AddDays(1);

        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        dateTimeProvider.UtcNow.Returns(todayMock);

        Club club = ClubFaker.Generate(1)[0];

        Player root = PlayerFaker.Generate(1)[0];

        SignUpPlayerAggregate sut = new(root, (club.Id, club.AnualBudget, 1000m));

        DateTime today = DateTime.Today;

        DateRange contractDuration = DateRange.Create(
            today, today.AddYears(3)).Value;

        // Action
        Result result = sut.SignUp(contractDuration, 1000m,
            signingWindow, dateTimeProvider);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OutOfSigningWindow);

        _ = dateTimeProvider.Received(1).UtcNow;
    }

    [Fact]
    public void Update_ShouldFail_WhenToday_IsLowerSigningWindow()
    {
        // Arrange
        DateTime today = DateTime.Today;

        DateRange signingWindow = DateRange.Create(
            today.AddMonths(-2), today.AddMonths(1)).Value;

        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        DateTime todayMock = signingWindow.Start.AddDays(-1);

        dateTimeProvider.UtcNow.Returns(todayMock);

        Club club = ClubFaker.Generate(1)[0];

        Player root = PlayerFaker.Generate(1)[0];

        SignUpPlayerAggregate sut = new(root, (club.Id, club.AnualBudget, 1000m));

        DateRange contractDuration = DateRange.Create(
            today, today.AddYears(3)).Value;

        // Action
        Result result = sut.SignUp(contractDuration, 1000m, 
            signingWindow, dateTimeProvider);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OutOfSigningWindow);
        
        _ = dateTimeProvider.Received(1).UtcNow;
    }
}
