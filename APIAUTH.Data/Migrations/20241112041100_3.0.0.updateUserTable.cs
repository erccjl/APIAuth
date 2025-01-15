using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIAUTH.Data.Migrations
{
    /// <inheritdoc />
    public partial class _300updateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneric",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BackupEmail",
                table: "Collaborators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Collaborators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGeneric",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BackupEmail",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Collaborators");

            migrationBuilder.AddColumn<string>(
                name: "BackupEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
