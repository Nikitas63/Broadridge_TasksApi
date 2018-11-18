using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.DataLayer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "TaskModel",
                schema: "dbo",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false, defaultValueSql: "sysdatetimeoffset()"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false, defaultValueSql: "sysdatetimeoffset()"),
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    TimeToComplete = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModel", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            // Data seeding
            migrationBuilder.Sql(@"
                DECLARE @Count INT = 1
                DECLARE @TaskName NVARCHAR(MAX)
                DECLARE @TaskDescription NVARCHAR(MAX)

                WHILE @Count <= 10000
                BEGIN
                    SET @TaskName = 'Task ' + CAST(@Count AS NVARCHAR(MAX))
                    SET @TaskDescription = 'Task Description Number ' + CAST(@Count AS NVARCHAR(MAX))

                    INSERT INTO [dbo].[TaskModel] 
                           ([Id],    [CreatedDate], [UpdatedDate], [Name],    [Description],    [Priority],             [TimeToComplete],         [Status], [IsDeleted])
                    VALUES (NEWID(), GETDATE(),     GETDATE(),     @TaskName, @TaskDescription, CAST(RAND()*10 as INT), CAST(RAND()*1000 as INT), 1,        0)

                    SET @Count = @Count + 1
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskModel",
                schema: "dbo");
        }
    }
}
