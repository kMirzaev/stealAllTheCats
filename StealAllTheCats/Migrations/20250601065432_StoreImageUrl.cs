using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StealAllTheCats.Migrations
{
    /// <inheritdoc />
    public partial class StoreImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cats");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cats");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Cats",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
