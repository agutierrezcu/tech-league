using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable(builder =>
        {
            builder.HasCheckConstraint(
               "CK_FullName",
               $"{nameof(Player.FullName)} != ''");

            builder.HasCheckConstraint(
               "CK_NickName",
               $"{nameof(Player.NickName)} is null OR {nameof(Player.NickName)} != ''");
        });

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FullName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.NickName)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(t => t.BirthDate)
            .IsRequired(false);

        builder.HasIndex(t => t.FullName)
            .IsUnique(false);

        builder.HasIndex(t => t.NickName)
            .IsUnique();

        builder.Ignore(t => t.IsFreeAgent);

        builder.Navigation(t => t.CurrentContract)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
