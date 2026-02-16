using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipLogbook.API.Migrations
{
    /// <inheritdoc />
    public partial class RebuildWithAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyProgrammes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyProgrammes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyProgrammes_Faculties",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudyProgrammeId = table.Column<int>(type: "int", nullable: false),
                    YearOfStudy = table.Column<int>(type: "int", nullable: false),
                    InternshipPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InternshipDirector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    HostTutor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EvaluationQuality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluationCommunication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluationLearning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuggestedGrade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Companies",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Students_StudyProgrammes",
                        column: x => x.StudyProgrammeId,
                        principalTable: "StudyProgrammes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfActivity = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeFrame = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Venue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillsPracticed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DailyAct__3214EC071D55DE6E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyActivities_Students",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TaskSequencing = table.Column<int>(type: "int", nullable: true),
                    Documentation = table.Column<int>(type: "int", nullable: true),
                    TheoreticalKnowledge = table.Column<int>(type: "int", nullable: true),
                    ToolsUsage = table.Column<int>(type: "int", nullable: true),
                    Measurements = table.Column<int>(type: "int", nullable: true),
                    DataRecording = table.Column<int>(type: "int", nullable: true),
                    GraphicPlans = table.Column<int>(type: "int", nullable: true),
                    Techniques = table.Column<int>(type: "int", nullable: true),
                    SafetyFeatures = table.Column<int>(type: "int", nullable: true),
                    ManualAssembly = table.Column<int>(type: "int", nullable: true),
                    ElectricalAssembly = table.Column<int>(type: "int", nullable: true),
                    Design = table.Column<int>(type: "int", nullable: true),
                    Testing = table.Column<int>(type: "int", nullable: true),
                    CompanyKnowledge = table.Column<int>(type: "int", nullable: true),
                    CommunicationTeam = table.Column<int>(type: "int", nullable: true),
                    CommunicationClients = table.Column<int>(type: "int", nullable: true),
                    Reflection = table.Column<int>(type: "int", nullable: true),
                    EthicalConduct = table.Column<int>(type: "int", nullable: true),
                    OrgCulture = table.Column<int>(type: "int", nullable: true),
                    CareerAnalysis = table.Column<int>(type: "int", nullable: true),
                    EvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipEvaluations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyActivities_StudentId",
                table: "DailyActivities",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipEvaluations_StudentId",
                table: "InternshipEvaluations",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CompanyId",
                table: "Students",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudyProgrammeId",
                table: "Students",
                column: "StudyProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgrammes_FacultyId",
                table: "StudyProgrammes",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StudentId",
                table: "Users",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyActivities");

            migrationBuilder.DropTable(
                name: "InternshipEvaluations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "StudyProgrammes");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
