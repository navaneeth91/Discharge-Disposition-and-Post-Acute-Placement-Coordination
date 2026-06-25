using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DischargeDisposition_Backend.Migrations.Hospital
{
    /// <inheritdoc />
    public partial class AddPatientAssignmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.CreateTable(
                name: "PatientAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CareManagerId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnassignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_PatientAssignments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAssignments_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAssignments_Users_CareManagerId",
                        column: x => x.CareManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssignments_AssignedBy",
                table: "PatientAssignments",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssignments_CareManagerId",
                table: "PatientAssignments",
                column: "CareManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssignments_PatientId",
                table: "PatientAssignments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDelays_Users_UserId",
                table: "PatientDelays",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
