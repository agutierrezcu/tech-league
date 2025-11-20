using SharedKernel;

namespace Domain.Players.SignUp;

public static class SignUpPlayerAggregateErrors
{
    public static readonly Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given id was not found.");

    public static readonly Error PlayerNotFound =
        Error.NotFound("PlayerNotFound", "Player with given id was not found.");

    public static readonly Error OutOfSigningWindow =
        Error.Conflict("OutOfSigningWindow", "Out of signing window.");

    public static readonly Error PlayedAlreadySigned =
        Error.Conflict("PlayedAlreadySigned", "Player is already signed by the club.");

    public static readonly Error PlayerUnderContractWithDifferentClub =
        Error.Conflict("PlayerUnderContractWithDifferentClub", "Player signed with another club.");

    public static readonly Error InsufficientRemainingBudget =
        Error.Conflict("InsufficientRemainingBudget", "Insufficient remaining budget to hire the Coach.");
}
