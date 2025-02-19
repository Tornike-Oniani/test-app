using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class updated_results : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultImageVariantTimes");

            migrationBuilder.DropColumn(
                name: "Recognized",
                table: "ResultImageSetTimes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recognized",
                table: "ResultImageSetTimes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ResultImageVariantTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageVariantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    Seconds = table.Column<double>(type: "REAL", nullable: false),
                    Skipped = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultImageVariantTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultImageVariantTimes_ImageVariants_ImageVariantId",
                        column: x => x.ImageVariantId,
                        principalTable: "ImageVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultImageVariantTimes_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultImageVariantTimes_ImageVariantId",
                table: "ResultImageVariantTimes",
                column: "ImageVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultImageVariantTimes_ResultId",
                table: "ResultImageVariantTimes",
                column: "ResultId");
        }
    }
}
