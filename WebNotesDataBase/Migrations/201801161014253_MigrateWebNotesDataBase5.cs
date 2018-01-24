namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            AlterColumn("dbo.Notes", "Label", c => c.String());
            AlterColumn("dbo.Notes", "Body", c => c.String());
            AlterColumn("dbo.Users", "NameAuthor", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Pass", c => c.String());
            AddForeignKey("dbo.Notes", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            AlterColumn("dbo.Users", "Pass", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Users", "NameAuthor", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Notes", "Body", c => c.String(nullable: false));
            AlterColumn("dbo.Notes", "Label", c => c.String(nullable: false, maxLength: 20));
            AddForeignKey("dbo.Notes", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
