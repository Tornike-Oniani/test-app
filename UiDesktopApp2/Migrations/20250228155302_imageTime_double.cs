using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class imageTime_double : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AvailableTimePerImageVariant",
                table: "Results",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AvailableTimePerImageVariant",
                table: "Results",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
