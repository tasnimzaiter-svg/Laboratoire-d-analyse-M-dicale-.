using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interface.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assurances",
                columns: table => new
                {
                    idAssurance = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nomAssurance = table.Column<string>(type: "TEXT", nullable: false),
                    tauxPriseEnCharge = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assurances", x => x.idAssurance);
                });

            migrationBuilder.CreateTable(
                name: "Echantillons",
                columns: table => new
                {
                    codeBarres = table.Column<string>(type: "TEXT", nullable: false),
                    typeEchantillon = table.Column<string>(type: "TEXT", nullable: false),
                    dateReception = table.Column<DateTime>(type: "TEXT", nullable: false),
                    statut = table.Column<string>(type: "TEXT", nullable: false),
                    motifRejet = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Echantillons", x => x.codeBarres);
                });

            migrationBuilder.CreateTable(
                name: "Medecins",
                columns: table => new
                {
                    idMedecin = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    specialite = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medecins", x => x.idMedecin);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    idUtilisateur = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    prenom = table.Column<string>(type: "TEXT", nullable: false),
                    login = table.Column<string>(type: "TEXT", nullable: false),
                    motDePasse = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Administrateur_matricule = table.Column<string>(type: "TEXT", nullable: true),
                    Comptable_matricule = table.Column<string>(type: "TEXT", nullable: true),
                    Secretaire_matricule = table.Column<string>(type: "TEXT", nullable: true),
                    matricule = table.Column<string>(type: "TEXT", nullable: true),
                    specialite = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.idUtilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    numFacture = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    dateFacture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    montantTotal = table.Column<double>(type: "REAL", nullable: false),
                    partAssurance = table.Column<double>(type: "REAL", nullable: false),
                    partPatient = table.Column<double>(type: "REAL", nullable: false),
                    statut = table.Column<string>(type: "TEXT", nullable: false),
                    assuranceidAssurance = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.numFacture);
                    table.ForeignKey(
                        name: "FK_Factures_Assurances_assuranceidAssurance",
                        column: x => x.assuranceidAssurance,
                        principalTable: "Assurances",
                        principalColumn: "idAssurance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    idPatient = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    prenom = table.Column<string>(type: "TEXT", nullable: false),
                    telephone = table.Column<string>(type: "TEXT", nullable: false),
                    idAssurance = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.idPatient);
                    table.ForeignKey(
                        name: "FK_Patients_Assurances_idAssurance",
                        column: x => x.idAssurance,
                        principalTable: "Assurances",
                        principalColumn: "idAssurance",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                columns: table => new
                {
                    idPaiement = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    datePaiement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    montant = table.Column<double>(type: "REAL", nullable: false),
                    modePaiement = table.Column<string>(type: "TEXT", nullable: false),
                    FacturenumFacture = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.idPaiement);
                    table.ForeignKey(
                        name: "FK_Paiements_Factures_FacturenumFacture",
                        column: x => x.FacturenumFacture,
                        principalTable: "Factures",
                        principalColumn: "numFacture");
                });

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    idAnalyse = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    libelle = table.Column<string>(type: "TEXT", nullable: false),
                    tarif = table.Column<double>(type: "REAL", nullable: false),
                    dateDemande = table.Column<DateTime>(type: "TEXT", nullable: false),
                    statut = table.Column<string>(type: "TEXT", nullable: false),
                    valeur = table.Column<string>(type: "TEXT", nullable: false),
                    dateSaisie = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modeSaisie = table.Column<string>(type: "TEXT", nullable: false),
                    valide = table.Column<bool>(type: "INTEGER", nullable: false),
                    patientidPatient = table.Column<int>(type: "INTEGER", nullable: true),
                    medecinidMedecin = table.Column<int>(type: "INTEGER", nullable: true),
                    echantilloncodeBarres = table.Column<string>(type: "TEXT", nullable: true),
                    facturenumFacture = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.idAnalyse);
                    table.ForeignKey(
                        name: "FK_Analyses_Echantillons_echantilloncodeBarres",
                        column: x => x.echantilloncodeBarres,
                        principalTable: "Echantillons",
                        principalColumn: "codeBarres",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Analyses_Factures_facturenumFacture",
                        column: x => x.facturenumFacture,
                        principalTable: "Factures",
                        principalColumn: "numFacture",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Analyses_Medecins_medecinidMedecin",
                        column: x => x.medecinidMedecin,
                        principalTable: "Medecins",
                        principalColumn: "idMedecin",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Analyses_Patients_patientidPatient",
                        column: x => x.patientidPatient,
                        principalTable: "Patients",
                        principalColumn: "idPatient");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_echantilloncodeBarres",
                table: "Analyses",
                column: "echantilloncodeBarres");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_facturenumFacture",
                table: "Analyses",
                column: "facturenumFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_medecinidMedecin",
                table: "Analyses",
                column: "medecinidMedecin");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_patientidPatient",
                table: "Analyses",
                column: "patientidPatient");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_assuranceidAssurance",
                table: "Factures",
                column: "assuranceidAssurance");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_FacturenumFacture",
                table: "Paiements",
                column: "FacturenumFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_idAssurance",
                table: "Patients",
                column: "idAssurance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "Paiements");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Echantillons");

            migrationBuilder.DropTable(
                name: "Medecins");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "Assurances");
        }
    }
}
