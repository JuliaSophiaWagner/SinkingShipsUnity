using Microsoft.EntityFrameworkCore.Migrations;

namespace SinkingShipsServer.Database.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllRegisteredPlayers",
                columns: table => new
                {
                    PrimaryKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Won = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllRegisteredPlayers", x => x.PrimaryKey);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PrimaryKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientDataPrimaryKey = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PrimaryKey);
                    table.ForeignKey(
                        name: "FK_Player_AllRegisteredPlayers_ClientDataPrimaryKey",
                        column: x => x.ClientDataPrimaryKey,
                        principalTable: "AllRegisteredPlayers",
                        principalColumn: "PrimaryKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_ClientDataPrimaryKey",
                table: "Player",
                column: "ClientDataPrimaryKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "AllRegisteredPlayers");
        }
    }
}
