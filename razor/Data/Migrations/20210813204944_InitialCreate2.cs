using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace razor.Data.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Masks",
                columns: table => new
                {
                    MaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mask = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masks", x => x.MaskID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "Vlans",
                columns: table => new
                {
                    VlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vlan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vlans", x => x.VlanID);
                });

            migrationBuilder.CreateTable(
                name: "Networks",
                columns: table => new
                {
                    NetworkID = table.Column<int>(type: "int", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AllocatedIP = table.Column<int>(type: "int", nullable: false),
                    MaskID = table.Column<int>(type: "int", nullable: false),
                    Nomadizm = table.Column<bool>(type: "bit", nullable: false),
                    InUseNet = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Networks", x => x.NetworkID);
                    table.ForeignKey(
                        name: "FK_Networks_Masks_MaskID",
                        column: x => x.MaskID,
                        principalTable: "Masks",
                        principalColumn: "MaskID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetworkAssignments",
                columns: table => new
                {
                    NetworkAssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NetworkID = table.Column<int>(type: "int", nullable: false),
                    VlanID = table.Column<int>(type: "int", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MNGMNT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    InUse = table.Column<bool>(type: "bit", nullable: false),
                    arp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    arpUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkAssignments", x => x.NetworkAssignmentID);
                    table.ForeignKey(
                        name: "FK_NetworkAssignments_Networks_NetworkID",
                        column: x => x.NetworkID,
                        principalTable: "Networks",
                        principalColumn: "NetworkID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetworkAssignments_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetworkAssignments_Vlans_VlanID",
                        column: x => x.VlanID,
                        principalTable: "Vlans",
                        principalColumn: "VlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAssignments_NetworkID",
                table: "NetworkAssignments",
                column: "NetworkID");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAssignments_VendorID",
                table: "NetworkAssignments",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAssignments_VlanID",
                table: "NetworkAssignments",
                column: "VlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Networks_MaskID",
                table: "Networks",
                column: "MaskID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetworkAssignments");

            migrationBuilder.DropTable(
                name: "Networks");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Vlans");

            migrationBuilder.DropTable(
                name: "Masks");
        }
    }
}
