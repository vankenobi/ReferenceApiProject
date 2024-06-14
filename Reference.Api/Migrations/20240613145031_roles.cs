using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reference.Api.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "Roles",
                table: "Users",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");
        }
    }
}
