using Domain.Clubs;
using Domain.Clubs.UpdateBudget;
using SharedKernel;
using Shouldly;
using static Domain.Clubs.Club;
using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace UnitTests;

public class UpdateBudgetAggregateUnittTest
{
    private const decimal DontCare = 0m;

    [Fact]
    public void Update_ShouldFail_WhenNewAnualBudget_IsBellowMinimumRequired()
    {
        // Arrange
        const decimal newAnualBudget = LeagueMinimumBudgetRequired - 1;

        Club club = new()
        {
            AnualBudget = LeagueMinimumBudgetRequired,
            Name = "Name",
            ThreeLettersName = "NAM"
        };

        UpdateAnualBudgetAggregate sut = new(club, DontCare);

        // Action
        Result result = sut.Update(newAnualBudget);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ValueBelowMinimumRequired);
    }

    [Fact]
    public void Update_ShouldFail_WhenNewAnualBudget_IsBellowCommittedBudget()
    {
        // Arrange
        const decimal currentAnualBudget = LeagueMinimumBudgetRequired * 2;
        const decimal committedBudget = LeagueMinimumBudgetRequired + 5;
        const decimal newAnualBudget = LeagueMinimumBudgetRequired + 2;

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
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ValueBellowCommittedBudget);
    }

    [Fact]
    public void Update_ShouldSucceed()
    {
        // Arrange
        const decimal currentAnualBudget = LeagueMinimumBudgetRequired * 2;
        const decimal committedBudget = LeagueMinimumBudgetRequired;
        const decimal newAnualBudget = LeagueMinimumBudgetRequired + 2;

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
        result.IsSuccess.ShouldBeTrue();
        club.AnualBudget.ShouldBe(newAnualBudget);
    }
}
