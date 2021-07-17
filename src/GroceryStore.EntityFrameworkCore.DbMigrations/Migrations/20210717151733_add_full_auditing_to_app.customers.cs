using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceryStore.Migrations
{
    public partial class add_full_auditing_to_appcustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "App",
                table: "Customers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "App",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                schema: "App",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                schema: "App",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                schema: "App",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                schema: "App",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "App",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "App",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                schema: "App",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                schema: "App",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                schema: "App",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                schema: "App",
                table: "Customers");
        }
    }
}
