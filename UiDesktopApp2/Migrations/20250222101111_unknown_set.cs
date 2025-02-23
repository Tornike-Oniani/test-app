using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class unknown_set : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUnknown",
                table: "ImageSets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnknown",
                table: "ImageSets");
        }
    }
}
