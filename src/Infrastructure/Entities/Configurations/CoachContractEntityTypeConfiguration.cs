using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entities.Configurations;

internal sealed class CoachContractEntityTypeConfiguration : IEntityTypeConfiguration<CoachContract>
{
    public void Configure(EntityTypeBuilder<CoachContract> builder)
    {
        builder.ToTable(builder =>
        {
            string constraintSql = string.Format(
                CultureInfo.CurrentCulture,
                "{0} <> '{1}' OR {2} >= {3}",
                ContractEntityTypeConfiguration.DescriminatorPropertyName,
                ContractTypeExtensions.ToStringFast(ContractType.Coach),
                nameof(Contract.AnualSalary),
                CoachContract.MinimumAnualSalary);

            builder.HasCheckConstraint(
                "CK_Coach_AnualSalary",
                constraintSql);
        });

        builder.HasOne(t => t.Coach)
            .WithOne(t => t.CurrentContract)
            .HasForeignKey<CoachContract>(t => t.CoachId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
