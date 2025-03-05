using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class imageset_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ImageSets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ImageSets");
        }
    }
}
