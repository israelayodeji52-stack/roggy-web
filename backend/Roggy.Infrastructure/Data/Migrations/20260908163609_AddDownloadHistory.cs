using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roggy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_download_history_songs_SongId",
                table: "download_history");

            migrationBuilder.DropForeignKey(
                name: "FK_download_history_users_UserId",
                table: "download_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_download_history",
                table: "download_history");

            migrationBuilder.DropIndex(
                name: "IX_download_history_DownloadedAt",
                table: "download_history");

            migrationBuilder.RenameTable(
                name: "download_history",
                newName: "download_histories");

            migrationBuilder.RenameIndex(
                name: "IX_download_history_UserId",
                table: "download_histories",
                newName: "IX_download_histories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_download_history_SongId",
                table: "download_histories",
                newName: "IX_download_histories_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_download_histories",
                table: "download_histories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_download_histories_UserId_DownloadedAt",
                table: "download_histories",
                columns: new[] { "UserId", "DownloadedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_download_histories_songs_SongId",
                table: "download_histories",
                column: "SongId",
                principalTable: "songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_download_histories_users_UserId",
                table: "download_histories",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_download_histories_songs_SongId",
                table: "download_histories");

            migrationBuilder.DropForeignKey(
                name: "FK_download_histories_users_UserId",
                table: "download_histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_download_histories",
                table: "download_histories");

            migrationBuilder.DropIndex(
                name: "IX_download_histories_UserId_DownloadedAt",
                table: "download_histories");

            migrationBuilder.RenameTable(
                name: "download_histories",
                newName: "download_history");

            migrationBuilder.RenameIndex(
                name: "IX_download_histories_UserId",
                table: "download_history",
                newName: "IX_download_history_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_download_histories_SongId",
                table: "download_history",
                newName: "IX_download_history_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_download_history",
                table: "download_history",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_download_history_DownloadedAt",
                table: "download_history",
                column: "DownloadedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_download_history_songs_SongId",
                table: "download_history",
                column: "SongId",
                principalTable: "songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_download_history_users_UserId",
                table: "download_history",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
