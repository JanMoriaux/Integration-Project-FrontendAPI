using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FrontEndAPI.Migrations
{
    public partial class IncludeIsActiveInUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_events",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    description = table.Column<string>(type: "text", nullable: false),
                    endtime = table.Column<DateTime>(nullable: false),
                    imageurl = table.Column<string>(maxLength: 500, nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    lastupdated = table.Column<DateTime>(type: "timestamp", nullable: false),
                    name = table.Column<string>(maxLength: 500, nullable: false),
                    starttime = table.Column<DateTime>(nullable: false),
                    uuid = table.Column<string>(maxLength: 36, nullable: false),
                    version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_users",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    bus = table.Column<string>(maxLength: 24, nullable: true),
                    city = table.Column<string>(maxLength: 500, nullable: false),
                    email = table.Column<string>(maxLength: 500, nullable: false),
                    emailverified = table.Column<bool>(nullable: false),
                    firstname = table.Column<string>(maxLength: 500, nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    lastupdated = table.Column<DateTime>(type: "timestamp", nullable: false),
                    lastname = table.Column<string>(maxLength: 500, nullable: false),
                    number = table.Column<int>(maxLength: 500, nullable: false),
                    password = table.Column<string>(maxLength: 500, nullable: false),
                    roles = table.Column<string>(maxLength: 30, nullable: false),
                    salt = table.Column<string>(maxLength: 100, nullable: false),
                    street = table.Column<string>(maxLength: 500, nullable: false),
                    uuid = table.Column<string>(maxLength: 36, nullable: false),
                    version = table.Column<long>(nullable: false),
                    zipcode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_activities",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    description = table.Column<string>(type: "text", nullable: false),
                    endtime = table.Column<DateTime>(nullable: false),
                    eventid = table.Column<long>(nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    lastupdated = table.Column<DateTime>(type: "timestamp", nullable: false),
                    name = table.Column<string>(maxLength: 500, nullable: false),
                    price = table.Column<decimal>(nullable: false),
                    remainingcapacity = table.Column<int>(nullable: false),
                    speakerid = table.Column<long>(nullable: true),
                    starttime = table.Column<DateTime>(nullable: false),
                    uuid = table.Column<string>(maxLength: 36, nullable: false),
                    version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_activities", x => x.id);
                    table.ForeignKey(
                        name: "FK_api_activities_api_events_eventid",
                        column: x => x.eventid,
                        principalTable: "api_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_api_activities_api_users_speakerid",
                        column: x => x.speakerid,
                        principalTable: "api_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "api_reservations",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    activityid = table.Column<long>(nullable: false),
                    hasattended = table.Column<bool>(nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    lastupdated = table.Column<DateTime>(type: "timestamp", nullable: false),
                    payedfee = table.Column<bool>(nullable: false),
                    uuid = table.Column<string>(maxLength: 36, nullable: false),
                    version = table.Column<long>(nullable: false),
                    visitorid = table.Column<long>(nullable: false),
                    withinvoice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_api_reservations_api_activities_activityid",
                        column: x => x.activityid,
                        principalTable: "api_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_api_reservations_api_users_visitorid",
                        column: x => x.visitorid,
                        principalTable: "api_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_api_activities_eventid",
                table: "api_activities",
                column: "eventid");

            migrationBuilder.CreateIndex(
                name: "IX_api_activities_speakerid",
                table: "api_activities",
                column: "speakerid");

            migrationBuilder.CreateIndex(
                name: "IX_api_activities_uuid_isactive",
                table: "api_activities",
                columns: new[] { "uuid", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_activities_name_uuid_isactive",
                table: "api_activities",
                columns: new[] { "name", "uuid", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_events_name_isactive",
                table: "api_events",
                columns: new[] { "name", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_events_uuid_isactive",
                table: "api_events",
                columns: new[] { "uuid", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_reservations_visitorid",
                table: "api_reservations",
                column: "visitorid");

            migrationBuilder.CreateIndex(
                name: "IX_api_reservations_uuid_isactive",
                table: "api_reservations",
                columns: new[] { "uuid", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_reservations_activityid_visitorid_isactive",
                table: "api_reservations",
                columns: new[] { "activityid", "visitorid", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_users_email_isactive",
                table: "api_users",
                columns: new[] { "email", "isactive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_users_uuid_isactive",
                table: "api_users",
                columns: new[] { "uuid", "isactive" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_reservations");

            migrationBuilder.DropTable(
                name: "api_activities");

            migrationBuilder.DropTable(
                name: "api_events");

            migrationBuilder.DropTable(
                name: "api_users");
        }
    }
}
