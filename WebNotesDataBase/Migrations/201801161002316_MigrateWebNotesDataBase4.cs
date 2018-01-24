namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            AddForeignKey("dbo.Notes", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "UserId", "dbo.Users");
            AddForeignKey("dbo.Notes", "UserId", "dbo.Users", "UserId");
        }
    }
}
