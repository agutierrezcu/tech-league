using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class FirstMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clubs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ThreeLettersName = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                AnualBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CommittedAnualBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clubs", x => x.Id);
                table.CheckConstraint("CK_Anual_Budget", "AnualBudget >= 1000000");
                table.CheckConstraint("CK_Name", "Name != ''");
                table.CheckConstraint("CK_ThreeLettersName", "ThreeLettersName != ''");
            });

        migrationBuilder.CreateTable(
            name: "Coaches",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Experience = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Coaches", x => x.Id);
                table.CheckConstraint("CK_Experience", "Experience >= 5");
                table.CheckConstraint("CK_FullName", "FullName != ''");
            });

        migrationBuilder.CreateTable(
            name: "Players",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                NickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Players", x => x.Id);
                table.CheckConstraint("CK_FullName1", "FullName != ''");
                table.CheckConstraint("CK_NickName", "NickName is null OR NickName != ''");
            });

        migrationBuilder.CreateTable(
            name: "FinanceStatusProjection",
            columns: table => new
            {
                ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AnualBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CommittedInPlayers = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CommittedInCoaches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                PlayerContractCount = table.Column<int>(type: "int", nullable: false),
                CoachContractCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FinanceStatusProjection", x => x.ClubId);
                table.ForeignKey(
                    name: "FK_FinanceStatusProjection_Clubs_ClubId",
                    column: x => x.ClubId,
                    principalTable: "Clubs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Contracts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AnualSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                Duration_End = table.Column<DateTime>(type: "datetime2", nullable: false),
                Duration_Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                CoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contracts", x => x.Id);
                table.CheckConstraint("CK_Coach_AnualSalary", "Type <> 'Coach' OR AnualSalary >= 1000");
                table.CheckConstraint("CK_Contract_Type", "Type = 'Player' OR Type = 'Coach'");
                table.CheckConstraint("CK_Player_AnualSalary", "Type <> 'Player' OR AnualSalary >= 10000");
                table.ForeignKey(
                    name: "FK_Contracts_Clubs_ClubId",
                    column: x => x.ClubId,
                    principalTable: "Clubs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Contracts_Coaches_CoachId",
                    column: x => x.CoachId,
                    principalTable: "Coaches",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_Contracts_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clubs_Name",
            table: "Clubs",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Clubs_ThreeLettersName",
            table: "Clubs",
            column: "ThreeLettersName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Coaches_FullName",
            table: "Coaches",
            column: "FullName");

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_ClubId",
            table: "Contracts",
            column: "ClubId");

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_CoachId",
            table: "Contracts",
            column: "CoachId",
            unique: true,
            filter: "[CoachId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_PlayerId",
            table: "Contracts",
            column: "PlayerId",
            unique: true,
            filter: "[PlayerId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Players_FullName",
            table: "Players",
            column: "FullName");

        migrationBuilder.CreateIndex(
            name: "IX_Players_NickName",
            table: "Players",
            column: "NickName",
            unique: true,
            filter: "[NickName] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Contracts");

        migrationBuilder.DropTable(
            name: "FinanceStatusProjection");

        migrationBuilder.DropTable(
            name: "Coaches");

        migrationBuilder.DropTable(
            name: "Players");

        migrationBuilder.DropTable(
            name: "Clubs");
    }
}
