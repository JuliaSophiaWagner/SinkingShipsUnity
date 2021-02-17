using Microsoft.EntityFrameworkCore.Migrations;

namespace SinkingShipsServer.Database.Migrations
{
    public partial class SinkingShipsDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinkingShipsPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passwort = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinkingShipsPlayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(type: "int", nullable: false),
                    CountShot = table.Column<int>(type: "int", nullable: false),
                    ShipHasSunk = table.Column<bool>(type: "bit", nullable: false),
                    GameInstancesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ship_GameInstances_GameInstancesId",
                        column: x => x.GameInstancesId,
                        principalTable: "GameInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GridElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XCoordinate = table.Column<int>(type: "int", nullable: false),
                    YCoordinate = table.Column<int>(type: "int", nullable: false),
                    HasBeenShot = table.Column<bool>(type: "bit", nullable: false),
                    IsShip = table.Column<bool>(type: "bit", nullable: false),
                    GameInstancesId = table.Column<int>(type: "int", nullable: true),
                    ShipId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridElement_GameInstances_GameInstancesId",
                        column: x => x.GameInstancesId,
                        principalTable: "GameInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GridElement_Ship_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GridElement_GameInstancesId",
                table: "GridElement",
                column: "GameInstancesId");

            migrationBuilder.CreateIndex(
                name: "IX_GridElement_ShipId",
                table: "GridElement",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_GameInstancesId",
                table: "Ship",
                column: "GameInstancesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GridElement");

            migrationBuilder.DropTable(
                name: "SinkingShipsPlayers");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropTable(
                name: "GameInstances");
        }
    }
}
