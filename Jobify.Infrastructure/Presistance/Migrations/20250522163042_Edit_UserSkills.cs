using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_UserSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_AspNetUsers_JobSeekerId",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_JobSeekerId",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "UserSkills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSeekerId",
                table: "UserSkills",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_JobSeekerId",
                table: "UserSkills",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_AspNetUsers_JobSeekerId",
                table: "UserSkills",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
