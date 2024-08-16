using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BatchProcess.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Bap",
                schema: "dbo",
                columns: table => new
                {
                    Bap_Id = table.Column<Guid>(type: "guid", nullable: false, comment: "Bap's Id."),
                    Bap_Code = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Bap's code."),
                    Bap_State = table.Column<int>(type: "int", nullable: false, comment: "0=Αρχική, 1=Σε εξέλιξη, 2=Διακόπηκε, 3=Απέτυχε, 4=Ολοκληρώθηκε"),
                    Bap_Started_DateTime = table.Column<DateOnly>(type: "datetime", nullable: true, comment: "Baps started date time."),
                    Bap_Cancelled_DateTime = table.Column<DateOnly>(type: "datetime", nullable: true, comment: "Baps cancelled date time."),
                    Bap_Finished_DateTime = table.Column<DateOnly>(type: "datetime", nullable: true, comment: "Baps finish date time."),
                    Bap_Failed_DateTime = table.Column<DateOnly>(type: "datetime", nullable: true, comment: "Baps failure date time."),
                    Bap_Session_Id = table.Column<Guid>(type: "guid", nullable: true, comment: "Baps session id."),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "Name of the user who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bap_BapId", x => x.Bap_Id);
                },
                comment: "Batch Process notifications");

            migrationBuilder.CreateTable(
                name: "Post",
                schema: "dbo",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false, comment: "Primary key for post records.")
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "integer", nullable: false, comment: "Key for post user records."),
                    Title = table.Column<string>(type: "varchar", maxLength: 255, nullable: false, comment: "Post's title."),
                    Body = table.Column<string>(type: "varchar", maxLength: 255, nullable: false, comment: "Post's body description."),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_PostId", x => x.PostId);
                },
                comment: "Posts for BatchProcess");

            migrationBuilder.CreateTable(
                name: "BapN",
                schema: "dbo",
                columns: table => new
                {
                    BapN_Id = table.Column<Guid>(type: "guid", nullable: false, comment: "BapN's Id."),
                    BapN_BapId = table.Column<Guid>(type: "guid", nullable: false, comment: "BapN's BapId."),
                    BapN_AA = table.Column<int>(type: "integer", nullable: false, comment: "BapN's BapNAA."),
                    BapN_DateTime = table.Column<DateOnly>(type: "datetime", nullable: true, comment: "Date and time bapn was created."),
                    BapN_Kind = table.Column<int>(type: "integer", nullable: false, comment: "BapN's kind."),
                    BapN_Data = table.Column<string>(type: "varchar", maxLength: 100, nullable: true, comment: "Bap's data."),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "Name of the user who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BapN_BapNId", x => x.BapN_Id);
                    table.ForeignKey(
                        name: "FK_BapN_Bap_BapId",
                        column: x => x.BapN_BapId,
                        principalSchema: "dbo",
                        principalTable: "Bap",
                        principalColumn: "Bap_Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Batch Process notifications details");

            migrationBuilder.CreateIndex(
                name: "AK_Bap_BapId",
                schema: "dbo",
                table: "Bap",
                column: "Bap_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_BapN_BapNId",
                schema: "dbo",
                table: "BapN",
                column: "BapN_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BapN_BapN_BapId",
                schema: "dbo",
                table: "BapN",
                column: "BapN_BapId");

            migrationBuilder.CreateIndex(
                name: "AK_Post_PostId",
                schema: "dbo",
                table: "Post",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BapN",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Post",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Bap",
                schema: "dbo");
        }
    }
}
