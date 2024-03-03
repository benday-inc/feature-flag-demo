using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Benday.Presidents.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                });

            /****************************************************/
            /* CREATE FEATURE FLAG RECORDS IN THE FEATURE TABLE */
            /****************************************************/
            migrationBuilder.Sql("INSERT [Feature] (IsEnabled, Name) VALUES (0, 'Search')");
            migrationBuilder.Sql("INSERT [Feature] (IsEnabled, Name) VALUES (0, 'SearchByBirthDeathState')");
            migrationBuilder.Sql("INSERT [Feature] (IsEnabled, Name) VALUES (0, 'FeatureUsageLogging')");
            migrationBuilder.Sql("INSERT [Feature] (IsEnabled, Name) VALUES (0, 'CustomerSatisfaction')");
            /****************************************************/

            migrationBuilder.CreateTable(
                name: "LogEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FeatureName = table.Column<string>(nullable: true),
                    LogDate = table.Column<DateTime>(nullable: false),
                    LogType = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ReferrerUrl = table.Column<string>(nullable: true),
                    RequestIpAddress = table.Column<string>(nullable: true),
                    RequestUrl = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonFact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FactType = table.Column<string>(nullable: true),
                    FactValue = table.Column<string>(nullable: true),
                    PersonId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonFact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonFact_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromPersonId = table.Column<int>(nullable: false),
                    RelationshipType = table.Column<string>(maxLength: 100, nullable: false),
                    ToPersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationship_Person_FromPersonId",
                        column: x => x.FromPersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relationship_Person_ToPersonId",
                        column: x => x.ToPersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonFact_PersonId",
                table: "PersonFact",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_FromPersonId",
                table: "Relationship",
                column: "FromPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_ToPersonId",
                table: "Relationship",
                column: "ToPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "LogEntry");

            migrationBuilder.DropTable(
                name: "PersonFact");

            migrationBuilder.DropTable(
                name: "Relationship");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
