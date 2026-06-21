using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WokoutRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Workouts",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Workouts");
        }
    }
}
