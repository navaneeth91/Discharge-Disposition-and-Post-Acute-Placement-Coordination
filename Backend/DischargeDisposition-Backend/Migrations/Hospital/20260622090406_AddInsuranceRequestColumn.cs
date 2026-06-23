using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DischargeDisposition_Backend.Migrations.Hospital
{
    /// <inheritdoc />
    public partial class AddInsuranceRequestColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.AddColumn<int>(
                name: "InsuranceAuthorizationRequestId",
                table: "AuthorizationTrackings",
                type: "int",
                nullable: true);
        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "InsuranceAuthorizationRequestId",
            table: "AuthorizationTrackings");
        }
    }
}
