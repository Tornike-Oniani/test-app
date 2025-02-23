using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UiDesktopApp2.Migrations
{
    /// <inheritdoc />
    public partial class subjcets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_People_PersonId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Results",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_PersonId",
                table: "Results",
                newName: "IX_Results_SubjectId");

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Subjects_SubjectId",
                table: "Results",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Subjects_SubjectId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Results",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_SubjectId",
                table: "Results",
                newName: "IX_Results_PersonId");

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Results_People_PersonId",
                table: "Results",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
