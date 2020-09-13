using Microsoft.EntityFrameworkCore.Migrations;

namespace GeMS_Key_Plus.Migrations
{
    public partial class InitializeProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buttons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hotkey = table.Column<string>(nullable: true),
                    ButtonName = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Template = table.Column<string>(nullable: true),
                    RequireSplit = table.Column<bool>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    SpecialDelimiters = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buttons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buttons");
        }
    }
}
