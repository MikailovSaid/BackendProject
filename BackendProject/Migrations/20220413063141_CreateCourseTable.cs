using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendProject.Migrations
{
    public partial class CreateCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    AboutCourse = table.Column<string>(nullable: false),
                    AboutDesc = table.Column<string>(nullable: false),
                    ApplyCourse = table.Column<string>(nullable: true),
                    ApplyDesc = table.Column<string>(nullable: true),
                    CertificationCourse = table.Column<string>(nullable: true),
                    CertificationDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
