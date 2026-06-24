using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DischargeDisposition_Backend.Migrations.Hospital
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Departments_departmentDeptId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_departmentDeptId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "departmentDeptId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DeptId",
                table: "Patients",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Departments_DeptId",
                table: "Patients",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Departments_DeptId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DeptId",
                table: "Patients");

            migrationBuilder.AddColumn<byte>(
                name: "departmentDeptId",
                table: "Patients",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_departmentDeptId",
                table: "Patients",
                column: "departmentDeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Departments_departmentDeptId",
                table: "Patients",
                column: "departmentDeptId",
                principalTable: "Departments",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
