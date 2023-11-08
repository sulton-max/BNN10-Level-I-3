using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N71_C.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4941dec3-36f6-412d-babc-abd92b79b9bb"),
                column: "Age",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Authors");
        }
    }
}
