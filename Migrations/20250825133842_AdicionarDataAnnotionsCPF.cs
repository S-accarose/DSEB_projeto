using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSEB_projeto.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDataAnnotionsCPF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "AspNetUsers",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                table: "AspNetUsers");
        }
    }
}
