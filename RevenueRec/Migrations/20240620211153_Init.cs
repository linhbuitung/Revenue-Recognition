using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRec.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategory);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ClientType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRS = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PESEL = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    IdDiscount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsForUpFront = table.Column<bool>(type: "bit", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.IdDiscount);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    IdEmployee = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.IdEmployee);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystems",
                columns: table => new
                {
                    IdSoftwareSystem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentVersionInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearlyCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareSystems", x => x.IdSoftwareSystem);
                    table.ForeignKey(
                        name: "FK_SoftwareSystems_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "IdCategory");
                });

            migrationBuilder.CreateTable(
                name: "SoftwareVersions",
                columns: table => new
                {
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdSoftwareSystem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersions", x => x.IdSoftwareVersion);
                    table.ForeignKey(
                        name: "FK_SoftwareVersions_SoftwareSystems_IdSoftwareSystem",
                        column: x => x.IdSoftwareSystem,
                        principalTable: "SoftwareSystems",
                        principalColumn: "IdSoftwareSystem");
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    IdSoftwareSystem = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RenewalPeriod = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.IdSubscription);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SoftwareSystems_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystems",
                        principalColumn: "IdSoftwareSystem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpFrontContracts",
                columns: table => new
                {
                    IdUpFrontContract = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PossibleUpdate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    ContractStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupportYears = table.Column<int>(type: "int", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdSoftwareSystem = table.Column<int>(type: "int", nullable: false),
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpFrontContracts", x => x.IdUpFrontContract);
                    table.ForeignKey(
                        name: "FK_UpFrontContracts_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "IdClient");
                    table.ForeignKey(
                        name: "FK_UpFrontContracts_SoftwareSystems_IdSoftwareSystem",
                        column: x => x.IdSoftwareSystem,
                        principalTable: "SoftwareSystems",
                        principalColumn: "IdSoftwareSystem");
                    table.ForeignKey(
                        name: "FK_UpFrontContracts_SoftwareVersions_IdSoftwareVersion",
                        column: x => x.IdSoftwareVersion,
                        principalTable: "SoftwareVersions",
                        principalColumn: "IdSoftwareVersion");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    IdPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUpFrontContract = table.Column<int>(type: "int", nullable: true),
                    IdSubscription = table.Column<int>(type: "int", nullable: true),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_Payments_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "IdClient");
                    table.ForeignKey(
                        name: "FK_Payments_Subscriptions_IdSubscription",
                        column: x => x.IdSubscription,
                        principalTable: "Subscriptions",
                        principalColumn: "IdSubscription");
                    table.ForeignKey(
                        name: "FK_Payments_UpFrontContracts_IdUpFrontContract",
                        column: x => x.IdUpFrontContract,
                        principalTable: "UpFrontContracts",
                        principalColumn: "IdUpFrontContract");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_KRS",
                table: "Clients",
                column: "KRS",
                unique: true,
                filter: "[KRS] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PESEL",
                table: "Clients",
                column: "PESEL",
                unique: true,
                filter: "[PESEL] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdClient",
                table: "Payments",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdSubscription",
                table: "Payments",
                column: "IdSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdUpFrontContract",
                table: "Payments",
                column: "IdUpFrontContract");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystems_IdCategory",
                table: "SoftwareSystems",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_IdSoftwareSystem",
                table: "SoftwareVersions",
                column: "IdSoftwareSystem");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ClientId",
                table: "Subscriptions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SoftwareSystemId",
                table: "Subscriptions",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_UpFrontContracts_IdClient",
                table: "UpFrontContracts",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_UpFrontContracts_IdSoftwareSystem",
                table: "UpFrontContracts",
                column: "IdSoftwareSystem");

            migrationBuilder.CreateIndex(
                name: "IX_UpFrontContracts_IdSoftwareVersion",
                table: "UpFrontContracts",
                column: "IdSoftwareVersion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UpFrontContracts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "SoftwareVersions");

            migrationBuilder.DropTable(
                name: "SoftwareSystems");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
