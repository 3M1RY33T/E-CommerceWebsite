using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseStudy.Migrations
{
    /// <inheritdoc />
    public partial class orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "OrderLineItems",
                newName: "CostPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CostPrice",
                table: "OrderLineItems",
                newName: "SellingPrice");
        }
    }
}
