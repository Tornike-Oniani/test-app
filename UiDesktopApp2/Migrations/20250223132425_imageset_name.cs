using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class imageset_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ImageSets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ImageSets");
        }
    }
}
