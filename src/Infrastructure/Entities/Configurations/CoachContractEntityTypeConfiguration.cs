using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class CoachContractEntityTypeConfiguration : IEntityTypeConfiguration<CoachContract>
{
    public void Configure(EntityTypeBuilder<CoachContract> builder)
    {
        builder.HasOne(t => t.Coach)
            .WithOne(t => t.CurrentContract)
            .HasForeignKey<CoachContract>(t => t.CoachId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
