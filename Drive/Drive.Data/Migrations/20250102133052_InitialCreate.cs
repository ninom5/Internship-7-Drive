using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Drive.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    HashedPassword = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Folders_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    FolderId = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    SharedWithId = table.Column<int>(type: "integer", nullable: false),
                    SharedById = table.Column<int>(type: "integer", nullable: false),
                    SharedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedItems_Files_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItems_Folders_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItems_Users_SharedById",
                        column: x => x.SharedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItems_Users_SharedWithId",
                        column: x => x.SharedWithId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "Name", "Password", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(417), "ante@gmail.com", new byte[] { 122, 89, 55, 159, 97, 165, 243, 126, 178, 86, 67, 235, 147, 97, 141, 112, 87, 128, 185, 121, 194, 211, 234, 95, 120, 103, 190, 181, 41, 26, 168, 254, 131, 161, 107, 172, 238, 129, 19, 114, 11, 151, 62, 190, 0, 194, 139, 130, 177, 212, 191, 8, 254, 115, 16, 56, 46, 192, 96, 254, 254, 53, 120, 167 }, "Ante", "ante123", "Antic" },
                    { 2, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(440), "mate@gmail.com", new byte[] { 73, 113, 152, 34, 172, 33, 62, 225, 188, 192, 152, 87, 138, 5, 57, 222, 211, 30, 164, 62, 229, 147, 168, 225, 215, 53, 176, 134, 210, 18, 201, 129, 35, 204, 147, 183, 210, 138, 74, 198, 105, 168, 206, 94, 227, 3, 217, 24, 203, 199, 125, 245, 78, 87, 104, 28, 62, 131, 4, 143, 182, 89, 121, 19 }, "Mate", "mate123", "Matic" },
                    { 3, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(457), "jonjones@gmail.com", new byte[] { 129, 57, 95, 117, 145, 114, 216, 10, 184, 63, 131, 92, 12, 83, 70, 101, 164, 212, 186, 141, 64, 64, 142, 63, 194, 180, 122, 230, 196, 113, 21, 112, 116, 90, 81, 205, 17, 249, 173, 14, 158, 220, 194, 37, 219, 176, 245, 84, 32, 148, 164, 218, 243, 201, 37, 56, 91, 178, 66, 185, 77, 138, 91, 56 }, "Jon", "jonjones123", "Jones" },
                    { 4, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(472), "curry@gmail.com", new byte[] { 79, 183, 97, 195, 214, 222, 118, 180, 185, 78, 163, 125, 171, 143, 196, 19, 31, 28, 159, 118, 86, 90, 67, 11, 109, 42, 179, 198, 74, 142, 193, 35, 239, 67, 167, 47, 132, 38, 4, 98, 61, 186, 13, 170, 224, 148, 72, 192, 64, 213, 127, 68, 33, 117, 208, 57, 159, 165, 215, 92, 65, 32, 245, 179 }, "Steph", "curry123", "Curry" },
                    { 5, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(487), "luka@basketball.com", new byte[] { 111, 88, 198, 77, 1, 224, 216, 125, 20, 127, 13, 7, 199, 241, 184, 30, 221, 6, 31, 141, 149, 92, 142, 120, 185, 16, 180, 186, 54, 238, 85, 236, 178, 76, 168, 229, 205, 147, 171, 75, 245, 202, 205, 177, 64, 169, 128, 48, 177, 79, 3, 82, 68, 73, 147, 196, 91, 54, 128, 30, 69, 178, 235, 184 }, "Luka", "luka123", "Dončić" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "CreatedAt", "LastModifiedAt", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(559), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Root Folder", 1, null },
                    { 2, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(562), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Root Folder", 2, null },
                    { 3, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(563), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Root Folder", 3, null },
                    { 4, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(564), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Root Folder", 4, null },
                    { 11, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(575), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Root Folder", 5, null }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedAt", "FolderId", "LastModifiedAt", "Name", "OwnerId" },
                values: new object[] { 1, "insert into Users(id) Values(1)", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(614), 1, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(614), "insert.sql", 1 });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "CreatedAt", "LastModifiedAt", "Name", "OwnerId", "ParentFolderId" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(566), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Projects", 1, 1 },
                    { 7, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(569), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "College", 1, 1 },
                    { 8, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(570), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "FESB", 2, 2 },
                    { 9, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(572), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Training", 3, 3 },
                    { 10, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(574), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Basketball Training", 4, 4 },
                    { 12, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(576), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Research", 5, 11 },
                    { 13, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(578), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personal", 5, 11 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "FileId", "LastModifiedAt", "UserId" },
                values: new object[] { 1, "The SQL script looks great, but I think we could optimize it a bit for larger datasets", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(690), 1, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(690), 1 });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedAt", "FolderId", "LastModifiedAt", "Name", "OwnerId" },
                values: new object[,]
                {
                    { 2, "public static class{\nint n = 1;\n}", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(617), 5, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(618), "Drive.cs", 1 },
                    { 5, "Bubble sort", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(623), 7, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(623), "Algorithms", 1 },
                    { 6, "tree node", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(628), 8, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(628), "Strukture podataka", 2 },
                    { 7, "Bench 3x15 - 100kg\n Deadlifts 2x10", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(630), 9, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(630), "Training of strength", 3 },
                    { 8, "30min", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(631), 9, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(632), "Cardio", 3 },
                    { 9, "Research on AI and Data Structures", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(633), 12, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(634), "ResearchPapers.pdf", 5 },
                    { 10, "Important personal notes", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(636), 13, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(636), "PersonalNotes.txt", 5 }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "CreatedAt", "LastModifiedAt", "Name", "OwnerId", "ParentFolderId" },
                values: new object[] { 6, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(568), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Database", 1, 5 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "FileId", "LastModifiedAt", "UserId" },
                values: new object[,]
                {
                    { 3, "This is a great file!", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(696), 9, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(697), 5 },
                    { 4, "I like the projections in this file.", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(698), 10, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(699), 5 }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Content", "CreatedAt", "FolderId", "LastModifiedAt", "Name", "OwnerId" },
                values: new object[,]
                {
                    { 3, "Create table", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(619), 6, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(620), "FitnessCenter", 1 },
                    { 4, "Create table", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(621), 6, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(622), "Restaurant", 1 }
                });

            migrationBuilder.InsertData(
                table: "SharedItems",
                columns: new[] { "Id", "ItemId", "ItemType", "SharedAt", "SharedById", "SharedWithId" },
                values: new object[] { 1, 5, 0, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(665), 1, 2 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "FileId", "LastModifiedAt", "UserId" },
                values: new object[] { 2, "The database structure is good, but can you clarify the indexing strategy used here?", new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(694), 3, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(695), 2 });

            migrationBuilder.InsertData(
                table: "SharedItems",
                columns: new[] { "Id", "ItemId", "ItemType", "SharedAt", "SharedById", "SharedWithId" },
                values: new object[] { 2, 3, 1, new DateTime(2025, 1, 2, 13, 30, 52, 641, DateTimeKind.Utc).AddTicks(667), 1, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FileId",
                table: "Comments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FolderId",
                table: "Files",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerId",
                table: "Files",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId",
                table: "Folders",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItems_ItemId",
                table: "SharedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItems_SharedById",
                table: "SharedItems",
                column: "SharedById");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItems_SharedWithId",
                table: "SharedItems",
                column: "SharedWithId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "SharedItems");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
