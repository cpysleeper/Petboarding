namespace CozyPawsPetboarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            DropColumn("dbo.AspNetUsers", "EmployeeId");
        }
    }
}
