using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookAppoinmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class newchangesindb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_healthCareCenters_HealthCareCenterId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_SlotStartTime_Status",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_HealthCareCenterId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SlotEndTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SlotStartTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "HealthCareCenterId",
                table: "Appointments",
                newName: "AvailabilityId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatusId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppointmentStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatus", x => x.StatusId);
                });

            migrationBuilder.InsertData(
                table: "AppointmentStatus",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Confirmed" },
                    { 3, "Rejected" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentStatusId",
                table: "Appointments",
                column: "AppointmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AvailabilityId",
                table: "Appointments",
                column: "AvailabilityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AvailabilityId_AppointmentStatusId",
                table: "Appointments",
                columns: new[] { "AvailabilityId", "AppointmentStatusId" },
                unique: true,
                filter: "[AppointmentStatusId] IN (1,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatus_AppointmentStatusId",
                table: "Appointments",
                column: "AppointmentStatusId",
                principalTable: "AppointmentStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Availabilities_AvailabilityId",
                table: "Appointments",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "AvailabilityId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentStatus_AppointmentStatusId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Availabilities_AvailabilityId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentStatus");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentStatusId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AvailabilityId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AvailabilityId_AppointmentStatusId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentStatusId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "Appointments",
                newName: "HealthCareCenterId");

            migrationBuilder.AddColumn<int>(
                name: "IsAvailable",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SlotEndTime",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SlotStartTime",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_SlotStartTime_Status",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate", "SlotStartTime", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HealthCareCenterId",
                table: "Appointments",
                column: "HealthCareCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_healthCareCenters_HealthCareCenterId",
                table: "Appointments",
                column: "HealthCareCenterId",
                principalTable: "healthCareCenters",
                principalColumn: "CenterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
