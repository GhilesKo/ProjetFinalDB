using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppProjetSessionDB.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Disponibilites");

            migrationBuilder.EnsureSchema(
                name: "Photos");

            migrationBuilder.EnsureSchema(
                name: "Utilisateurs");

            migrationBuilder.EnsureSchema(
                name: "Proprietes");

            migrationBuilder.CreateTable(
                name: "Photographes",
                schema: "Utilisateurs",
                columns: table => new
                {
                    photographeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photographes", x => x.photographeID);
                });

            migrationBuilder.CreateTable(
                name: "Propriete",
                schema: "Proprietes",
                columns: table => new
                {
                    proprieteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adresse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ville = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    nomProprio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    telProprio = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriete", x => x.proprieteID);
                });

            migrationBuilder.CreateTable(
                name: "RendezVous",
                schema: "Disponibilites",
                columns: table => new
                {
                    rendezVousID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateRendezVous = table.Column<DateTime>(type: "datetime2", nullable: false),
                    commentaire = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    proprieteID = table.Column<int>(type: "int", nullable: false),
                    heureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    justification = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    statutPhoto = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValueSql: "('Aucune')"),
                    commentairePhotos = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rendezVousID", x => x.rendezVousID);
                    table.ForeignKey(
                        name: "FK_proprieteID",
                        column: x => x.proprieteID,
                        principalSchema: "Proprietes",
                        principalTable: "Propriete",
                        principalColumn: "proprieteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disponibilite",
                schema: "Disponibilites",
                columns: table => new
                {
                    disponibiliteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateDisponibilite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    photographeID = table.Column<int>(type: "int", nullable: false),
                    heureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    rendezVousID = table.Column<int>(type: "int", nullable: true),
                    statut = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilite", x => x.disponibiliteID);
                    table.ForeignKey(
                        name: "FK_photographeID",
                        column: x => x.photographeID,
                        principalSchema: "Utilisateurs",
                        principalTable: "Photographes",
                        principalColumn: "photographeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rendezVousID",
                        column: x => x.rendezVousID,
                        principalSchema: "Disponibilites",
                        principalTable: "RendezVous",
                        principalColumn: "rendezVousID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                schema: "Photos",
                columns: table => new
                {
                    photoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rendezVousID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.photoID);
                    table.ForeignKey(
                        name: "FK_Photo_rendezVousID",
                        column: x => x.rendezVousID,
                        principalSchema: "Disponibilites",
                        principalTable: "RendezVous",
                        principalColumn: "rendezVousID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dispo_rdvID",
                schema: "Disponibilites",
                table: "Disponibilite",
                column: "rendezVousID");

            migrationBuilder.CreateIndex(
                name: "UC_photographe_date_heure",
                schema: "Disponibilites",
                table: "Disponibilite",
                columns: new[] { "photographeID", "dateDisponibilite", "heureDebut" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_RendezVousID",
                schema: "Photos",
                table: "Photo",
                column: "rendezVousID");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_proprieteID",
                schema: "Disponibilites",
                table: "RendezVous",
                column: "proprieteID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disponibilite",
                schema: "Disponibilites");

            migrationBuilder.DropTable(
                name: "Photo",
                schema: "Photos");

            migrationBuilder.DropTable(
                name: "Photographes",
                schema: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "RendezVous",
                schema: "Disponibilites");

            migrationBuilder.DropTable(
                name: "Propriete",
                schema: "Proprietes");
        }
    }
}
