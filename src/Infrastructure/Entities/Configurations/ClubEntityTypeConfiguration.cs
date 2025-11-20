using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class ClubEntityTypeConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable(builder =>
        {
            builder.HasCheckConstraint(
                "CK_Anual_Budget",
                $"{nameof(Club.AnualBudget)} >= {Club.MinimumAnualBudget}");

            builder.HasCheckConstraint(
                "CK_Name",
                $"{nameof(Club.Name)} != ''");

            builder.HasCheckConstraint(
                "CK_ThreeLettersName",
                $"{nameof(Club.ThreeLettersName)} != ''");
        });

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ThreeLettersName)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(t => t.AnualBudget)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasIndex(t => t.ThreeLettersName)
            .IsUnique();

        builder.Property<decimal>("CommittedAnualBudget")
            .HasColumnType("decimal(18,2)");
    }
}
