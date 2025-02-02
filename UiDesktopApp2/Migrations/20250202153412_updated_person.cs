using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class updated_person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "People",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_PersonId",
                table: "Results",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_People_PersonId",
                table: "Results",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_People_PersonId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_PersonId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "People",
                newName: "Name");
        }
    }
}
