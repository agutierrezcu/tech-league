using Bogus;
using Domain.Clubs;
using Domain.Players;
using Domain.Players.SignUp;
using Domain.ValueObjects;
using Moq;
using SharedKernel;
using Shouldly;
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

        Mock<IDateTimeProvider> dateTimeProviderMock = new();

        dateTimeProviderMock.Setup(x => x.UtcNow)
            .Returns(todayMock);

        Club club = ClubFaker.Generate(1)[0];

        Player player = PlayerFaker.Generate(1)[0];

        SignUpPlayerAggregateRoot root = new(club, player);

        SignUpPlayerAggregate sut = new(root, 1000m);

        DateRange contractDuration = DateRange.FromTodayTo(
            DateTime.Today.AddYears(3)).Value;

        // Action
        Result result = sut.SignUp(contractDuration, 1000m, signingWindow,
            dateTimeProviderMock.Object);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(OutOfSigningWindow);
        dateTimeProviderMock.Verify(x => x.UtcNow, Times.Once);
    }

    [Fact]
    public void Update_ShouldFail_WhenToday_IsLowerSigningWindow()
    {
        // Arrange
        DateRange signingWindow = DateRange.Create(DateTime.Today.AddMonths(-2),
            DateTime.Today.AddMonths(1)).Value;

        DateTime todayMock = signingWindow.Start.AddDays(-1);

        Mock<IDateTimeProvider> dateTimeProviderMock = new();

        dateTimeProviderMock.Setup(x => x.UtcNow)
            .Returns(todayMock);

        Club club = ClubFaker.Generate(1)[0];

        Player player = PlayerFaker.Generate(1)[0];

        SignUpPlayerAggregateRoot root = new(club, player);

        SignUpPlayerAggregate sut = new(root, 1000m);

        DateRange contractDuration = DateRange.FromTodayTo(
            DateTime.Today.AddYears(3)).Value;

        // Action
        Result result = sut.SignUp(contractDuration, 1000m, signingWindow,
            dateTimeProviderMock.Object);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(OutOfSigningWindow);
        dateTimeProviderMock.Verify(x => x.UtcNow, Times.Once);
    }
}
