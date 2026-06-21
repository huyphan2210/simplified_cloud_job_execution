using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_cloud_job_execution_backend.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    project_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
