using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventsInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UtcTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "AdditionalInfo", "Category", "CreatedAt", "Date", "Description", "ImageUrl", "Place", "Recurrency", "Title", "UpdatedAt", "UtcTime" },
                values: new object[,]
                {
                    { new Guid("2a8c63b8-90fd-4b42-aecb-c1cf83332ca5"), "Additional info for Event 3", "Category 1", new DateTimeOffset(new DateTime(2023, 9, 12, 14, 0, 11, 957, DateTimeKind.Unspecified).AddTicks(5467), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(2023, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description of Event 3", "https://example.com/image3.jpg", "Place 3", null, "Event 3", null, new TimeSpan(0, 16, 0, 0, 0) },
                    { new Guid("2aa86b08-9a32-45e2-9fbd-407e3e27569f"), "Additional info for Event 1", "Category 1", new DateTimeOffset(new DateTime(2023, 9, 12, 14, 0, 11, 957, DateTimeKind.Unspecified).AddTicks(5431), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(2023, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description of Event 1", "https://example.com/image1.jpg", "Place 1", null, "Event 1", null, new TimeSpan(0, 18, 0, 0, 0) },
                    { new Guid("8d9326af-d0d4-4a49-a709-f214a1f97476"), "Additional info for Event 2", "Category 2", new DateTimeOffset(new DateTime(2023, 9, 12, 14, 0, 11, 957, DateTimeKind.Unspecified).AddTicks(5465), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(2023, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description of Event 2", "https://example.com/image2.jpg", "Place 2", null, "Event 2", null, new TimeSpan(0, 19, 30, 0, 0) },
                    { new Guid("ca7c8490-9226-4c8c-84c0-e69b77b16745"), "Additional info for Event 5", "Category 2", new DateTimeOffset(new DateTime(2023, 9, 12, 14, 0, 11, 957, DateTimeKind.Unspecified).AddTicks(5470), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(2023, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description of Event 5", "https://example.com/image5.jpg", "Place 5", null, "Event 5", null, new TimeSpan(0, 20, 0, 0, 0) },
                    { new Guid("e6bfa644-1bb8-4b9f-9be8-c3129e50ea32"), "Additional info for Event 4", "Category 3", new DateTimeOffset(new DateTime(2023, 9, 12, 14, 0, 11, 957, DateTimeKind.Unspecified).AddTicks(5468), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description of Event 4", "https://example.com/image4.jpg", "Place 4", null, "Event 4", null, new TimeSpan(0, 17, 0, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
