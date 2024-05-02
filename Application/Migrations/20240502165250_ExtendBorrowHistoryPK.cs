using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class ExtendBorrowHistoryPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowHistory",
                table: "BorrowHistory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "309ef301-9fab-4bd6-8a33-7a74c4a96d9b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb1e3b15-157b-4be4-b630-7f4f1a1d1273");

            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "BorrowHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowHistory",
                table: "BorrowHistory",
                columns: new[] { "UserId", "LibraryId", "BookIsbn", "BorrowDate" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bf1e327a-4440-48ad-8d63-0e5e8a43ddf3", null, "standard", "standard" },
                    { "f5809433-6559-4cdc-adf0-5b3ef41fda8e", null, "librarian", "librarian" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowHistory",
                table: "BorrowHistory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf1e327a-4440-48ad-8d63-0e5e8a43ddf3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5809433-6559-4cdc-adf0-5b3ef41fda8e");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "BorrowHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowHistory",
                table: "BorrowHistory",
                columns: new[] { "UserId", "BookIsbn", "BorrowDate" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "309ef301-9fab-4bd6-8a33-7a74c4a96d9b", null, "standard", "standard" },
                    { "eb1e3b15-157b-4be4-b630-7f4f1a1d1273", null, "librarian", "librarian" }
                });
        }
    }
}
