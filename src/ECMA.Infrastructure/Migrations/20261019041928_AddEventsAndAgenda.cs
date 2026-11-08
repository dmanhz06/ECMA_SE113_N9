using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECMA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsAndAgenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    start_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_by = table.Column<Guid>(type: "uuid", nullable: true),
                    cancel_reason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.CheckConstraint("ck_events_capacity", "capacity BETWEEN 1 AND 10000");
                    table.CheckConstraint("ck_events_cancellation", "(status = 2) = (cancelled_at IS NOT NULL AND cancelled_by IS NOT NULL AND cancel_reason IS NOT NULL)");
                    table.CheckConstraint("ck_events_minute_precision", "date_trunc('minute', start_at) = start_at AND date_trunc('minute', end_at) = end_at");
                    table.CheckConstraint("ck_events_not_published_deleted", "NOT (status = 1 AND is_deleted)");
                    table.CheckConstraint("ck_events_status", "status IN (0, 1, 2)");
                    table.CheckConstraint("ck_events_time", "end_at >= start_at + interval '5 minutes' AND end_at <= start_at + interval '30 days'");
                    table.CheckConstraint("ck_events_type", "type IN (0, 1)");
                    table.ForeignKey(
                        name: "fk_events_cancelled_by",
                        column: x => x.cancelled_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_events_owner",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "agenda_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    start_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_agenda_items", x => x.id);
                    table.CheckConstraint("ck_agenda_min_duration", "end_at >= start_at + interval '5 minutes'");
                    table.CheckConstraint("ck_agenda_minute_precision", "date_trunc('minute', start_at) = start_at AND date_trunc('minute', end_at) = end_at");
                    table.ForeignKey(
                        name: "FK_agenda_items_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_agenda_event_start",
                table: "agenda_items",
                columns: new[] { "event_id", "start_at" });

            migrationBuilder.CreateIndex(
                name: "IX_events_cancelled_by",
                table: "events",
                column: "cancelled_by");

            migrationBuilder.CreateIndex(
                name: "ix_events_created",
                table: "events",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_events_owner_created",
                table: "events",
                columns: new[] { "owner_id", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agenda_items");

            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
