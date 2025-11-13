using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistence.Identity.Migrations
{
    /// <inheritdoc />
    public partial class EditUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DispalyName",
                table: "Users",
                newName: "DisplayName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Users",
                newName: "DispalyName");
        }
    }
}
