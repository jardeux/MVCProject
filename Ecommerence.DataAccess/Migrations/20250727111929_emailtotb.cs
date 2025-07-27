using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerence.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class emailtotb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "orderHeaders");
        }
    }
}
