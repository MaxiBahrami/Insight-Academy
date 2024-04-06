using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsightAcademy.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnzipcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Zipcode",
                table: "TeacherProfile",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "TeacherProfile");
        }
    }
}
