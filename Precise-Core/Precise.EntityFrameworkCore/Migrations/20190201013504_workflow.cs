using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Precise.Migrations
{
    public partial class workflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowInstanceOperationHistorys",
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
                    InstanceId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstanceOperationHistorys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstances",
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
                    InstanceSchemeId = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CustomName = table.Column<string>(nullable: true),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityType = table.Column<int>(nullable: true),
                    ActivityName = table.Column<string>(nullable: true),
                    PreviousId = table.Column<string>(nullable: true),
                    SchemeContent = table.Column<string>(nullable: true),
                    SchemeId = table.Column<string>(nullable: true),
                    DbName = table.Column<string>(nullable: true),
                    FrmType = table.Column<int>(nullable: false),
                    FrmContentData = table.Column<string>(nullable: true),
                    FrmContentParse = table.Column<string>(nullable: true),
                    FrmId = table.Column<string>(nullable: true),
                    SchemeType = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    FlowLevel = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsFinish = table.Column<int>(nullable: false),
                    MakerList = table.Column<string>(nullable: true),
                    FrmData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstanceTransitionHistorys",
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
                    InstanceId = table.Column<string>(nullable: true),
                    FromNodeId = table.Column<string>(nullable: true),
                    FromNodeType = table.Column<int>(nullable: true),
                    FromNodeName = table.Column<string>(nullable: true),
                    ToNodeId = table.Column<string>(nullable: true),
                    ToNodeType = table.Column<int>(nullable: true),
                    ToNodeName = table.Column<string>(nullable: true),
                    TransitionSate = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstanceTransitionHistorys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowSchemes",
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
                    SchemeCode = table.Column<string>(nullable: true),
                    SchemeName = table.Column<string>(nullable: true),
                    SchemeType = table.Column<string>(nullable: true),
                    SchemeVersion = table.Column<string>(nullable: true),
                    SchemeCanUser = table.Column<string>(nullable: true),
                    SchemeContent = table.Column<string>(nullable: true),
                    FrmId = table.Column<string>(nullable: true),
                    FrmType = table.Column<int>(nullable: false),
                    AuthorizeType = table.Column<int>(nullable: false),
                    SortCode = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
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
                    Name = table.Column<string>(nullable: true),
                    Fields = table.Column<int>(nullable: false),
                    ContentData = table.Column<string>(nullable: true),
                    ContentParse = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    SortCode = table.Column<int>(nullable: false),
                    Delete = table.Column<int>(nullable: false),
                    DbName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowInstanceOperationHistorys");

            migrationBuilder.DropTable(
                name: "FlowInstances");

            migrationBuilder.DropTable(
                name: "FlowInstanceTransitionHistorys");

            migrationBuilder.DropTable(
                name: "FlowSchemes");

            migrationBuilder.DropTable(
                name: "Forms");
        }
    }
}
