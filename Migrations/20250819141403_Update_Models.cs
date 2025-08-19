using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSEB_projeto.Migrations
{
    /// <inheritdoc />
    public partial class Update_Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemString",
                table: "Servicos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEntrega",
                table: "Pedidos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemString",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "DataEntrega",
                table: "Pedidos");
        }
    }
}
