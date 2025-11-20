using SharedKernel;

namespace Application.Clubs.Players.Get;

public static class GetPlayerByClubErrors
{
    public static readonly Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given id was not found.");
}
