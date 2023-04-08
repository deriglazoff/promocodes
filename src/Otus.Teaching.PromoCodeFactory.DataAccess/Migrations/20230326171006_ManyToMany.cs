using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Employees_PartnerManagerId",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "PromoCodes",
                newName: "PromoCode");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "Preference");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodes_PreferenceId",
                table: "PromoCode",
                newName: "IX_PromoCode_PreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodes_PartnerManagerId",
                table: "PromoCode",
                newName: "IX_PromoCode_PartnerManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodes_CustomerId",
                table: "PromoCode",
                newName: "IX_PromoCode_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RoleId",
                table: "Employee",
                newName: "IX_Employee_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preference",
                table: "Preference",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CustomerPreference",
                columns: table => new
                {
                    CustomersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PreferencesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPreference", x => new { x.CustomersId, x.PreferencesId });
                    table.ForeignKey(
                        name: "FK_CustomerPreference_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPreference_Preference_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "Preference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomerPreference",
                columns: new[] { "CustomersId", "PreferencesId" },
                values: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("c4bda62e-fc74-4256-a956-4760b3858cbd") });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPreference_PreferencesId",
                table: "CustomerPreference",
                column: "PreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Role_RoleId",
                table: "Employee",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_Customer_CustomerId",
                table: "PromoCode",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_Employee_PartnerManagerId",
                table: "PromoCode",
                column: "PartnerManagerId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_Preference_PreferenceId",
                table: "PromoCode",
                column: "PreferenceId",
                principalTable: "Preference",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Role_RoleId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_Customer_CustomerId",
                table: "PromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_Employee_PartnerManagerId",
                table: "PromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_Preference_PreferenceId",
                table: "PromoCode");

            migrationBuilder.DropTable(
                name: "CustomerPreference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preference",
                table: "Preference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "PromoCode",
                newName: "PromoCodes");

            migrationBuilder.RenameTable(
                name: "Preference",
                newName: "Preferences");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCode_PreferenceId",
                table: "PromoCodes",
                newName: "IX_PromoCodes_PreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCode_PartnerManagerId",
                table: "PromoCodes",
                newName: "IX_PromoCodes_PartnerManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCode_CustomerId",
                table: "PromoCodes",
                newName: "IX_PromoCodes_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_RoleId",
                table: "Employees",
                newName: "IX_Employees_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Employees_PartnerManagerId",
                table: "PromoCodes",
                column: "PartnerManagerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id");
        }
    }
}
