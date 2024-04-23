using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class AddBookThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47964ff1-8ab5-4fc1-9649-af8903f7399a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bb8049b-a800-4580-b0fb-fc3f31e46ca3");

            migrationBuilder.DropColumn(
                name: "CoverPicture",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "309ef301-9fab-4bd6-8a33-7a74c4a96d9b", null, "standard", "standard" },
                    { "eb1e3b15-157b-4be4-b630-7f4f1a1d1273", null, "librarian", "librarian" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "309ef301-9fab-4bd6-8a33-7a74c4a96d9b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb1e3b15-157b-4be4-b630-7f4f1a1d1273");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Books");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverPicture",
                table: "Books",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47964ff1-8ab5-4fc1-9649-af8903f7399a", null, "standard", "standard" },
                    { "7bb8049b-a800-4580-b0fb-fc3f31e46ca3", null, "librarian", "librarian" }
                });
        }
    }
}
