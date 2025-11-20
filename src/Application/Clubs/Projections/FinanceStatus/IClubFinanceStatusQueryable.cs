namespace Application.Clubs.Projections.FinanceStatus;

public interface IClubFinanceStatusQueryable : IQueryableDbContext
{ 
    IQueryable<ClubFinanceStatusProjection> FinanceStatusProjections { get; }
}
