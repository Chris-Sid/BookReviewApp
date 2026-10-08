using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBookKeysetIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_Genre_Title_Id",
                table: "Books",
                columns: new[] { "Genre", "Title", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title_Id",
                table: "Books",
                columns: new[] { "Title", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_Genre_Title_Id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Title_Id",
                table: "Books");
        }
    }
}
