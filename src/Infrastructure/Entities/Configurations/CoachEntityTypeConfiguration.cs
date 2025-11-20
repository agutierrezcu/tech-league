using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class CoachEntityTypeConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable(builder =>
        {
            builder.HasCheckConstraint(
                "CK_Experience",
                $"{nameof(Coach.Experience)} >= {Coach.MinimumExperience}");

            builder.HasCheckConstraint(
               "CK_FullName",
               $"{nameof(Coach.FullName)} != ''");
        });

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FullName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Experience)
            .IsRequired();

        builder.HasIndex(t => t.FullName)
            .IsUnique(false);

        builder.Ignore(t => t.IsAvailable);

        builder.Navigation(t => t.CurrentContract)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
