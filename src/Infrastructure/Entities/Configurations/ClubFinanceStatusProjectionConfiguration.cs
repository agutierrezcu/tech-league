using Application.Clubs.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class ClubFinanceStatusProjectionConfiguration :
    IEntityTypeConfiguration<ClubFinanceStatusProjection>
{
    public void Configure(EntityTypeBuilder<ClubFinanceStatusProjection> builder)
    {
        builder.HasKey(p => p.ClubId);

        builder.Property(p => p.AnualBudget)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.CommittedInPlayers)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.CommittedInCoaches)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Ignore(p => p.RemainingAnualBudget);
        builder.Ignore(p => p.CommittedAnualBudget);
        builder.Ignore(p => p.TotalContractsCount);

        builder.HasOne(p => p.Club)
            .WithOne()
            .HasForeignKey<ClubFinanceStatusProjection>(p => p.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
