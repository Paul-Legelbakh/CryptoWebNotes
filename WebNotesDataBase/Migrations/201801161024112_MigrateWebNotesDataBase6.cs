namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notes", "Label", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Notes", "Body", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "NameAuthor", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Users", "Pass", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Pass", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "NameAuthor", c => c.String());
            AlterColumn("dbo.Notes", "Body", c => c.String());
            AlterColumn("dbo.Notes", "Label", c => c.String());
        }
    }
}
