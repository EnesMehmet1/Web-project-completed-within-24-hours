namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WA0001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeletedPersons",
                c => new
                    {
                        deletedPersonId = c.Int(nullable: false, identity: true),
                        personId = c.Int(nullable: false),
                        deletedPersonDeletedDate = c.DateTime(nullable: false),
                        AdminName = c.String(),
                    })
                .PrimaryKey(t => t.deletedPersonId)
                .ForeignKey("dbo.People", t => t.personId, cascadeDelete: true)
                .Index(t => t.personId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        personId = c.Int(nullable: false, identity: true),
                        personName = c.String(),
                        personSurname = c.String(),
                        personEMail = c.String(),
                        personPassword = c.String(),
                        PersonProfileImage = c.String(),
                        PersonShaNo = c.String(),
                        PersonState = c.Boolean(nullable: false),
                        personRegisterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.personId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeletedPersons", "personId", "dbo.People");
            DropIndex("dbo.DeletedPersons", new[] { "personId" });
            DropTable("dbo.People");
            DropTable("dbo.DeletedPersons");
        }
    }
}
