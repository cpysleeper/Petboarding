namespace CozyPawsPetboarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPendingModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "PetOwnerId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            CreateIndex("dbo.AspNetUsers", "PetOwnerId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "PetOwnerId", "dbo.PetOwners", "PetOwnerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PetOwnerId", "dbo.PetOwners");
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "PetOwnerId" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            DropColumn("dbo.AspNetUsers", "PetOwnerId");
            DropColumn("dbo.AspNetUsers", "EmployeeId");
        }
    }
}
