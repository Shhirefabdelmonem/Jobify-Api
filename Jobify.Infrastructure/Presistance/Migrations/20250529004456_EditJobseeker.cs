using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditJobseeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSeekerId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobSeekerId",
                table: "AspNetUsers",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobSeekers_JobSeekerId",
                table: "AspNetUsers",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobSeekers_JobSeekerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobSeekerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "AspNetUsers");
        }
    }
}
