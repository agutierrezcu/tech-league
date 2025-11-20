using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class PlayerContractEntityTypeConfiguration : IEntityTypeConfiguration<PlayerContract>
{
    public void Configure(EntityTypeBuilder<PlayerContract> builder)
    {
        builder.ToTable(builder =>
        {
            string constraintSql = string.Format(
                CultureInfo.CurrentCulture,
                "{0} <> '{1}' OR {2} >= {3}",
                ContractEntityTypeConfiguration.DescriminatorPropertyName,
                ContractTypeExtensions.ToStringFast(ContractType.Player),
                nameof(Contract.AnualSalary),
                PlayerContract.MinimumAnualSalary);

            builder.HasCheckConstraint(
                "CK_Player_AnualSalary",
                constraintSql);
        });

        builder.HasOne(t => t.Player)
            .WithOne(t => t.CurrentContract)
            .HasForeignKey<PlayerContract>(t => t.PlayerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
