using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Entities.Configurations;

internal sealed class ContractEntityTypeConfiguration : IEntityTypeConfiguration<Contract>
{
    internal const string DescriminatorPropertyName = "Type";

    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");

        string[] contractTypeNames = ContractTypeExtensions.GetNames();

        string checkLiteral = contractTypeNames
            .Aggregate(string.Empty, (result, name) =>
                result + $"{DescriminatorPropertyName} = '{name}' OR ");

        builder.ToTable(builder =>
        {
            builder.HasCheckConstraint(
                "CK_Contract_Type",
                checkLiteral[..(checkLiteral.Length - 4)]);
        });

        builder.HasKey(t => t.Id);

        builder.UseTphMappingStrategy()
            .HasDiscriminator<ContractType>(DescriminatorPropertyName)
            .HasValue<PlayerContract>(ContractType.Player)
            .HasValue<CoachContract>(ContractType.Coach);

        builder.Property(DescriminatorPropertyName)
            .HasMaxLength(contractTypeNames.Max(c => c.Length))
            .HasConversion(
                new ValueConverter<ContractType, string>(
                    v => v.ToStringFast(),
                    v => ContractTypeExtensions.Parse(v)));

        builder.ComplexProperty(t => t.Duration)
            .IsRequired();

        builder.Property(t => t.AnualSalary)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(t => t.Club)
            .WithMany(t => t.Contracts)
            .HasForeignKey(t => t.ClubId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
