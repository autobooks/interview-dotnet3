using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceryStore.Migrations
{
    public partial class Create_App_Customers_AltKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "App");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "App",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                        LegacyId = table.Column<int>(type: "int", nullable: false),
                        Name = table.Column<string>(
                            type: "nvarchar(255)",
                            maxLength: 255,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.UniqueConstraint("AK_Customers_LegacyId", x => x.LegacyId);
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Customers", schema: "App");
        }
    }
}
