using Microsoft.EntityFrameworkCore.Migrations;

namespace Diary.DAL.Migrations
{
    public partial class UpdateUploadedFileAddIsBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImage",
                table: "UploadedFiles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImage",
                table: "UploadedFiles");
        }
    }
}
