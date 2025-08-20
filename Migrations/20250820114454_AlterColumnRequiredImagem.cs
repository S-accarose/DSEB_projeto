using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSEB_projeto.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnRequiredImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagemString",
                table: "Servicos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ImagemString",
                keyValue: null,
                column: "ImagemString",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ImagemString",
                table: "Servicos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
