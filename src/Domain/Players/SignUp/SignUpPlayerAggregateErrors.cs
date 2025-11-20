using SharedKernel;

namespace Domain.Players.SignUp;

public static class SignUpPlayerAggregateErrors
{
    public readonly static Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given id was not found.");

    public readonly static Error PlayerNotFound =
        Error.NotFound("PlayerNotFound", "Player with given id was not found.");

    public readonly static Error OutOfSigningWindow =
        Error.Conflict("OutOfSigningWindow", "Out of signing window.");

    public readonly static Error PlayedAlreadySigned =
        Error.Conflict("PlayedAlreadySigned", "Player is already signed by the club.");

    public readonly static Error PlayerUnderContractWithDifferentClub =
        Error.Conflict("PlayerUnderContractWithDifferentClub", "Player signed with another club.");

    public readonly static Error InsufficientRemainingBudget =
        Error.Conflict("InsufficientRemainingBudget", "Insufficient remaining budget to hire the Coach.");
}
