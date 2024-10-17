using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationViewAndEmployeeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 1,
                column: "order_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4520));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 2,
                column: "order_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4562));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 3,
                column: "order_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 4,
                column: "order_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 5,
                column: "order_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4569));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 1,
                column: "reservation_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 2,
                column: "reservation_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 3,
                column: "reservation_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4618));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 4,
                column: "reservation_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4620));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 5,
                column: "reservation_date",
                value: new DateTime(2024, 9, 24, 13, 15, 40, 468, DateTimeKind.Local).AddTicks(4622));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 1,
                column: "order_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(682));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 2,
                column: "order_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(728));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 3,
                column: "order_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(731));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 4,
                column: "order_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(733));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 5,
                column: "order_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(735));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 1,
                column: "reservation_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(794));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 2,
                column: "reservation_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(797));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 3,
                column: "reservation_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(799));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 4,
                column: "reservation_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(801));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 5,
                column: "reservation_date",
                value: new DateTime(2024, 9, 23, 16, 40, 58, 112, DateTimeKind.Local).AddTicks(804));
        }
    }
}
