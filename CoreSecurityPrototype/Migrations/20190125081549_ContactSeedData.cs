using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreSecurityPrototype.Migrations
{
    public partial class ContactSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("0bf0a853-a243-491a-bb68-09cc94c16369"), "Gavin", "Roux" });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("51d98396-f0e5-4ec2-866f-8f6e8f23043a"), "Dean", "Cowell" });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("3dcde597-df95-4ee7-91f4-015799c11f29"), "Michelle", "Uys" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("0bf0a853-a243-491a-bb68-09cc94c16369"));

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("3dcde597-df95-4ee7-91f4-015799c11f29"));

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("51d98396-f0e5-4ec2-866f-8f6e8f23043a"));
        }
    }
}
