using Domain.Clubs;
using Domain.Players;
using Domain.Players.SignUp;
using Domain.ValueObjects;
using Moq;
using SharedKernel;
using Shouldly;

using static Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace UnitTests;

public class SignUpPlayerAggregateUnittTest
{
    [Fact]
    public void Update_ShouldFail_WhenToday_IsGreaterSigningWindow()
    {
        // Arrange
        var signingWindow = DateRange.From(DateTime.Today.AddMonths(-2),
            DateTime.Today.AddMonths(1));

        DateTime todayMock = signingWindow.End.AddDays(1);

        Mock<IDateTimeProvider> dateTimeProviderMock = new();

        dateTimeProviderMock.Setup(x => x.UtcNow)
            .Returns(todayMock);

        SignUpPlayerAggregateRoot root = new(new Club(), new Player());

        SignUpPlayerAggregate sut = new(root, 1000m);

        var contractDuration = DateRange.FromTodayTo(DateTime.Today.AddYears(3));

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
        var signingWindow = DateRange.From(DateTime.Today.AddMonths(-2),
             DateTime.Today.AddMonths(1));

        DateTime todayMock = signingWindow.Start.AddDays(-1);

        Mock<IDateTimeProvider> dateTimeProviderMock = new();

        dateTimeProviderMock.Setup(x => x.UtcNow)
            .Returns(todayMock);

        SignUpPlayerAggregateRoot root = new(new Club(), new Player());

        SignUpPlayerAggregate sut = new(root, 1000m);

        var contractDuration = DateRange.FromTodayTo(DateTime.Today.AddYears(3));

        // Action
        Result result = sut.SignUp(contractDuration, 1000m, signingWindow,
            dateTimeProviderMock.Object);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(OutOfSigningWindow);
        dateTimeProviderMock.Verify(x => x.UtcNow, Times.Once);
    }
}
