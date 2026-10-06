using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roggy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PromoteIsraelToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE users
                SET "Role" = 'Admin'
                WHERE "Email" = 'israel@example.com';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE users
                SET "Role" = 'User'
                WHERE "Email" = 'israel@example.com';
                """);
        }
    }
}