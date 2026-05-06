namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPetOwnerToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PetOwnerId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "PetOwnerId");
            AddForeignKey("dbo.AspNetUsers", "PetOwnerId", "dbo.PetOwners", "PetOwnerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PetOwnerId", "dbo.PetOwners");
            DropIndex("dbo.AspNetUsers", new[] { "PetOwnerId" });
            DropColumn("dbo.AspNetUsers", "PetOwnerId");
        }
    }
}
