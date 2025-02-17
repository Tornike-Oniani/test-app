using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class tracking_precision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Seconds",
                table: "ResultImageVariantTimes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "Skipped",
                table: "ResultImageVariantTimes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "Seconds",
                table: "ResultImageSetTimes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "Recognized",
                table: "ResultImageSetTimes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "People",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_TestId",
                table: "Results",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultImageVariantTimes_ImageVariantId",
                table: "ResultImageVariantTimes",
                column: "ImageVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultImageSetTimes_ImageSetId",
                table: "ResultImageSetTimes",
                column: "ImageSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultImageSetTimes_ImageSets_ImageSetId",
                table: "ResultImageSetTimes",
                column: "ImageSetId",
                principalTable: "ImageSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultImageVariantTimes_ImageVariants_ImageVariantId",
                table: "ResultImageVariantTimes",
                column: "ImageVariantId",
                principalTable: "ImageVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Tests_TestId",
                table: "Results",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultImageSetTimes_ImageSets_ImageSetId",
                table: "ResultImageSetTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultImageVariantTimes_ImageVariants_ImageVariantId",
                table: "ResultImageVariantTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Tests_TestId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_TestId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_ResultImageVariantTimes_ImageVariantId",
                table: "ResultImageVariantTimes");

            migrationBuilder.DropIndex(
                name: "IX_ResultImageSetTimes_ImageSetId",
                table: "ResultImageSetTimes");

            migrationBuilder.DropColumn(
                name: "Skipped",
                table: "ResultImageVariantTimes");

            migrationBuilder.DropColumn(
                name: "Recognized",
                table: "ResultImageSetTimes");

            migrationBuilder.AlterColumn<int>(
                name: "Seconds",
                table: "ResultImageVariantTimes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "Seconds",
                table: "ResultImageSetTimes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "People",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
