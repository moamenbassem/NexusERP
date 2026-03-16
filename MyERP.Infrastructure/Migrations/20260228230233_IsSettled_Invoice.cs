using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsSettled_Invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "IsSetteled",
                table: "Invoices",
                newName: "IsSettled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSettled",
                table: "Invoices",
                newName: "IsSetteled");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
