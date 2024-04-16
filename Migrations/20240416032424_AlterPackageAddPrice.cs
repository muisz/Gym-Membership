using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMembership.Migrations
{
    /// <inheritdoc />
    public partial class AlterPackageAddPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Packages",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Packages");
        }
    }
}
