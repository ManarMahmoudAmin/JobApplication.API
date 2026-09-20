using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCandidateApplicationKeyAndCancellation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateApplications",
                table: "CandidateApplications");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CandidateApplications",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "CandidateApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateApplications",
                table: "CandidateApplications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateApplications_CandidateId_JobId",
                table: "CandidateApplications",
                columns: new[] { "CandidateId", "JobId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateApplications",
                table: "CandidateApplications");

            migrationBuilder.DropIndex(
                name: "IX_CandidateApplications_CandidateId_JobId",
                table: "CandidateApplications");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CandidateApplications");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "CandidateApplications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateApplications",
                table: "CandidateApplications",
                columns: new[] { "CandidateId", "JobId" });
        }
    }
}
