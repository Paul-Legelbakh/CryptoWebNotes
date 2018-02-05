namespace WebNotesDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateWebNotesDataBase7 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 1000));
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 500));
            CreateIndex("dbo.Users", "Email", unique: true);
        }
    }
}
