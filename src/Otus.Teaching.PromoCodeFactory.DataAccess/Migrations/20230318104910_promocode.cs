using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    public partial class promocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "PromoCodes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_CustomerId",
                table: "PromoCodes",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_CustomerId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PromoCodes");
        }
    }
}
