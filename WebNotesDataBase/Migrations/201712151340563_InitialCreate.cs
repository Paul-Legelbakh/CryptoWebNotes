namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(),
                        EditedDate = c.DateTime(),
                        Label = c.String(nullable: false, maxLength: 20),
                        Body = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        NameAuthor = c.String(nullable: false, maxLength: 30),
                        Birthday = c.DateTime(nullable: false),
                        Email = c.String(nullable: false, maxLength: 40),
                        Pass = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            DropIndex("dbo.Notes", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Notes");
        }
    }
}
