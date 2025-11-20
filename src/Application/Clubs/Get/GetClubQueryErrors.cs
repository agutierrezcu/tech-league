using SharedKernel;

namespace Application.Clubs.Get;

public static class GetClubQueryErrors
{
    public static readonly Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given id was not found.");
}
