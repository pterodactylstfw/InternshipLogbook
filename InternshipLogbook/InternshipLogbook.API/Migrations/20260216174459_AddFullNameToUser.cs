using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipLogbook.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CoordinatorId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CoordinatorId",
                table: "Students",
                column: "CoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Coordinator",
                table: "Students",
                column: "CoordinatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Coordinator",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CoordinatorId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CoordinatorId",
                table: "Students");
        }
    }
}
