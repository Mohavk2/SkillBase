using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillBase.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Skills_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReferenceUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceUrl_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceUrl_SkillId",
                table: "ReferenceUrl",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ParentId",
                table: "Skills",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceUrl");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
