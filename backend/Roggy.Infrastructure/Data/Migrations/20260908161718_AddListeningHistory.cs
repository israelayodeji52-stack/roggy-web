using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roggy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddListeningHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_listening_history_songs_SongId",
                table: "listening_history");

            migrationBuilder.DropForeignKey(
                name: "FK_listening_history_users_UserId",
                table: "listening_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_listening_history",
                table: "listening_history");

            migrationBuilder.DropIndex(
                name: "IX_listening_history_StartedAt",
                table: "listening_history");

            migrationBuilder.RenameTable(
                name: "listening_history",
                newName: "listening_histories");

            migrationBuilder.RenameIndex(
                name: "IX_listening_history_UserId",
                table: "listening_histories",
                newName: "IX_listening_histories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_listening_history_SongId",
                table: "listening_histories",
                newName: "IX_listening_histories_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_listening_histories",
                table: "listening_histories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_listening_histories_UserId_StartedAt",
                table: "listening_histories",
                columns: new[] { "UserId", "StartedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_listening_histories_songs_SongId",
                table: "listening_histories",
                column: "SongId",
                principalTable: "songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_listening_histories_users_UserId",
                table: "listening_histories",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_listening_histories_songs_SongId",
                table: "listening_histories");

            migrationBuilder.DropForeignKey(
                name: "FK_listening_histories_users_UserId",
                table: "listening_histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_listening_histories",
                table: "listening_histories");

            migrationBuilder.DropIndex(
                name: "IX_listening_histories_UserId_StartedAt",
                table: "listening_histories");

            migrationBuilder.RenameTable(
                name: "listening_histories",
                newName: "listening_history");

            migrationBuilder.RenameIndex(
                name: "IX_listening_histories_UserId",
                table: "listening_history",
                newName: "IX_listening_history_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_listening_histories_SongId",
                table: "listening_history",
                newName: "IX_listening_history_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_listening_history",
                table: "listening_history",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_listening_history_StartedAt",
                table: "listening_history",
                column: "StartedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_listening_history_songs_SongId",
                table: "listening_history",
                column: "SongId",
                principalTable: "songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_listening_history_users_UserId",
                table: "listening_history",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
