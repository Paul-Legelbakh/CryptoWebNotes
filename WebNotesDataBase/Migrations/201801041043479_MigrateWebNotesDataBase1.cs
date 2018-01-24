namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Email" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "Email", unique: true);
        }
    }
}
