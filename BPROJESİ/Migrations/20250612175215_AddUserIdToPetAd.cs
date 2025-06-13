using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPROJESİ.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPetAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PetAds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PetAds");
        }
    }
}
