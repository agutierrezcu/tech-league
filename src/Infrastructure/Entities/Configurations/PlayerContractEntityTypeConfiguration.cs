using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class PlayerContractEntityTypeConfiguration : IEntityTypeConfiguration<PlayerContract>
{
    public void Configure(EntityTypeBuilder<PlayerContract> builder)
    {
        builder.HasOne(t => t.Player)
            .WithOne(t => t.CurrentContract)
            .HasForeignKey<PlayerContract>(t => t.PlayerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
