﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionColumnInModuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modules");
        }
    }
}
