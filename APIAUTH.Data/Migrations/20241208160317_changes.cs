using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIAUTH.Data.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Users",
                newName: "StateUser");

            migrationBuilder.RenameColumn(
                name: "IsGeneric",
                table: "Users",
                newName: "IsGenericPassword");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Collaborators",
                newName: "NumberPhone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateUser",
                table: "Users",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "IsGenericPassword",
                table: "Users",
                newName: "IsGeneric");

            migrationBuilder.RenameColumn(
                name: "NumberPhone",
                table: "Collaborators",
                newName: "Phone");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }
    }
}
