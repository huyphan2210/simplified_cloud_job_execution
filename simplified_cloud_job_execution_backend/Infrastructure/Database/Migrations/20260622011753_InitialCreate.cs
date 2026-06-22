using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace simplified_cloud_job_execution_backend.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    compute_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    input_file_name = table.Column<string>(type: "text", nullable: false),
                    input_file_reference = table.Column<string>(type: "text", nullable: true),
                    output_file_reference = table.Column<string>(type: "text", nullable: true),
                    execution_duration_seconds = table.Column<double>(type: "double precision", nullable: true),
                    credit_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    billing_processed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jobs", x => x.id);
                    table.ForeignKey(
                        name: "fk_jobs_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "id", "created_at", "description", "project_name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("3c9b2b6d-2a71-4023-8f9d-ea1b48a61520"), new DateTime(2026, 5, 5, 14, 45, 0, 0, DateTimeKind.Utc), "Model training and hyperparameter sweep workloads.", "ML Training", null },
                    { new Guid("9a3e8f89-0fd2-4f42-84c5-2b4a4aa232c1"), new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), "GPU-backed model inference and validation workloads.", "Inference Lab", null },
                    { new Guid("af23b61d-4d7e-4f58-9f38-6ccaf68fca74"), new DateTime(2026, 2, 2, 9, 30, 0, 0, DateTimeKind.Utc), "Batch ETL and analytics exports for engineering metrics.", "Analytics Pipeline", null },
                    { new Guid("d41f4b5a-5c8a-4dbe-a5d6-8f3a923b0d57"), new DateTime(2026, 3, 15, 12, 15, 0, 0, DateTimeKind.Utc), "Video and media rendering jobs for quality checks.", "Media Rendering", null },
                    { new Guid("f9e8b0a0-1c47-4472-bc7a-2e8e473cd8af"), new DateTime(2026, 4, 4, 10, 0, 0, 0, DateTimeKind.Utc), "Sensor aggregation and device telemetry processing.", "IoT Batch Jobs", null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_jobs_project_id",
                table: "Jobs",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
