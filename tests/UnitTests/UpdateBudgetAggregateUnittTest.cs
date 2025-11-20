using Domain.Clubs;
using Domain.Clubs.UpdateBudget;
using FluentAssertions;
using SharedKernel;
using static Domain.Clubs.Club;
using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace UnitTests;

public class UpdateBudgetAggregateUnittTest
{
    private const decimal DontCare = decimal.MinusOne;

    [Fact]
    public void Update_ShouldFail_WhenNewAnualBudget_IsBellowMinimumRequired()
    {
        // Arrange
        const decimal newAnualBudget = MinimumAnualBudget - 1;

        Club club = new()
        {
            AnualBudget = MinimumAnualBudget,
            Name = "Name",
            ThreeLettersName = "NAM"
        };

        UpdateAnualBudgetAggregate sut = new(club, DontCare);

        // Action
        Result result = sut.Update(newAnualBudget);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValueBelowMinimumRequired);
    }

    [Fact]
    public void Update_ShouldFail_WhenNewAnualBudget_IsBellowCommittedBudget()
    {
        // Arrange
        const decimal currentAnualBudget = MinimumAnualBudget * 2;
        const decimal committedBudget = MinimumAnualBudget + 5;
        const decimal newAnualBudget = MinimumAnualBudget + 2;

        Club club = new()
        {
            AnualBudget = currentAnualBudget,
            Name = "Name",
            ThreeLettersName = "NAM"
        };

        UpdateAnualBudgetAggregate sut = new(club, committedBudget);

        // Action
        Result result = sut.Update(newAnualBudget);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValueBellowCommittedBudget);
    }

    [Fact]
    public void Update_ShouldSucceed()
    {
        // Arrange
        const decimal currentAnualBudget = MinimumAnualBudget * 2;
        const decimal committedBudget = MinimumAnualBudget;
        const decimal newAnualBudget = MinimumAnualBudget + 2;

        Club club = new()
        {
            AnualBudget = currentAnualBudget,
            Name = "Name",
            ThreeLettersName = "NAM"
        };

        UpdateAnualBudgetAggregate sut = new(club, committedBudget);

        // Action
        Result result = sut.Update(newAnualBudget);

        // Assert
        result.IsSuccess.Should().BeTrue();
        club.AnualBudget.Should().Be(newAnualBudget);
    }
}
