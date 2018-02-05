namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "NameAuthor", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Users", "Pass", c => c.String());
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Pass", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "NameAuthor", c => c.String(nullable: false));
        }
    }
}
