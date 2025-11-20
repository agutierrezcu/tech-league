namespace Web.Api.Clubs.Coaches.Dismiss;

internal sealed record DismissCoachRequest(ClubId ClubId, CoachId CoachId);
