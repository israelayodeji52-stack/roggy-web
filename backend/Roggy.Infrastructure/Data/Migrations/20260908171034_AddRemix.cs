using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roggy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRemix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AudioUrl",
                table: "remixes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_remixes_IsPublished",
                table: "remixes",
                column: "IsPublished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_remixes_IsPublished",
                table: "remixes");

            migrationBuilder.AlterColumn<string>(
                name: "AudioUrl",
                table: "remixes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
