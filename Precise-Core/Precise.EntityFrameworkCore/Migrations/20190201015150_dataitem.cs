using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Precise.Migrations
{
    public partial class dataitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemsDetailEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ItemId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ItemCode = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: true),
                    SortCode = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsDetailEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemsEntitys",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    EnCode = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    SortCode = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsEntitys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsDetailEntities");

            migrationBuilder.DropTable(
                name: "ItemsEntitys");
        }
    }
}
