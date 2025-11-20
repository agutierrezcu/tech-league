using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class ContractEntityTypeConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");

        builder.HasKey(t => t.Id);

        builder
            .UseTphMappingStrategy()
            .HasDiscriminator<ContractType>("Type")
            .HasValue<PlayerContract>(ContractType.Player)
            .HasValue<CoachContract>(ContractType.Coach);

        builder
            .ComplexProperty(t => t.Duration)
            .IsRequired(true);

        builder
            .Property(t => t.AnualSalary)
            .HasColumnType("decimal(18,2)")
            .IsRequired(true);

        builder
           .HasOne(t => t.Club)
           .WithMany(t => t.Contracts)
           .HasForeignKey(t => t.ClubId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);
    }
}
