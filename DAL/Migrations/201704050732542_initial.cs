namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Hours = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Assignments", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Assignments", "MemberId", "dbo.Members");
            DropIndex("dbo.Members", new[] { "Project_Id" });
            DropIndex("dbo.Assignments", new[] { "MemberId" });
            DropIndex("dbo.Assignments", new[] { "ProjectId" });
            DropTable("dbo.Projects");
            DropTable("dbo.Members");
            DropTable("dbo.Assignments");
        }
    }
}
