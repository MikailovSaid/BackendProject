using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendProject.Migrations
{
    public partial class CreateTeacherContactAndSkillsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Skype = table.Column<string>(nullable: true),
                    TeacherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherContacts_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(nullable: true),
                    TeamLeader = table.Column<string>(nullable: true),
                    Development = table.Column<string>(nullable: true),
                    Design = table.Column<string>(nullable: true),
                    Innovation = table.Column<string>(nullable: true),
                    Communication = table.Column<string>(nullable: true),
                    TeacherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSkills_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherContacts_TeacherId",
                table: "TeacherContacts",
                column: "TeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSkills_TeacherId",
                table: "TeacherSkills",
                column: "TeacherId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherContacts");

            migrationBuilder.DropTable(
                name: "TeacherSkills");
        }
    }
}
