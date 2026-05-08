namespace CozyPawsPetboarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        BookingStartTime = c.DateTime(nullable: false),
                        BookingEndTime = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 50),
                        CheckInMethod = c.String(maxLength: 50),
                        CheckOutMethod = c.String(maxLength: 50),
                        CanceledAt = c.DateTime(),
                        CancellationReason = c.String(maxLength: 300),
                        SpecialInstructions = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        PetId = c.Guid(nullable: false),
                        PetOwnerId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Species = c.String(maxLength: 50),
                        Breed = c.String(maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Sex = c.String(maxLength: 20),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetId)
                .ForeignKey("dbo.PetOwners", t => t.PetOwnerId, cascadeDelete: true)
                .Index(t => t.PetOwnerId);
            
            CreateTable(
                "dbo.PetEmergencyContacts",
                c => new
                    {
                        PetEmergencyContactId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        FullName = c.String(maxLength: 150),
                        RelationshipToOwner = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 200),
                        Notes = c.String(maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetEmergencyContactId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.PetFeedingPlans",
                c => new
                    {
                        PetFeedingPlanId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        FeedingsPerDay = c.Int(nullable: false),
                        AmountPerFeeding = c.String(maxLength: 100),
                        FoodBrand = c.String(maxLength: 150),
                        FeedingInstructions = c.String(maxLength: 500),
                        HasSpecialDiet = c.Boolean(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetFeedingPlanId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.PetMedications",
                c => new
                    {
                        PetMedicationId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        MedicationName = c.String(maxLength: 200),
                        Dosage = c.String(maxLength: 100),
                        Frequency = c.String(maxLength: 100),
                        Instructions = c.String(maxLength: 500),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetMedicationId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.PetOwners",
                c => new
                    {
                        PetOwnerId = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 50),
                        PostalCode = c.String(maxLength: 20),
                        CreatedAt = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PetOwnerId);
            
            CreateTable(
                "dbo.ContactUsSubmissions",
                c => new
                    {
                        ContactUsSubmissionId = c.Guid(nullable: false),
                        PetOwnerId = c.Guid(),
                        FullName = c.String(maxLength: 150),
                        Email = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 20),
                        Subject = c.String(maxLength: 200),
                        Message = c.String(),
                        Status = c.String(maxLength: 50),
                        SubmittedAt = c.DateTime(nullable: false),
                        ResolvedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactUsSubmissionId)
                .ForeignKey("dbo.PetOwners", t => t.PetOwnerId)
                .Index(t => t.PetOwnerId);
            
            CreateTable(
                "dbo.BookingStatusHistories",
                c => new
                    {
                        BookingStatusHistoryId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                        EmployeeId = c.Guid(nullable: false),
                        Status = c.String(maxLength: 50),
                        StatusAt = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.BookingStatusHistoryId)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 200),
                        Role = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BookingStatusHistories", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.BookingStatusHistories", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.Pets", "PetOwnerId", "dbo.PetOwners");
            DropForeignKey("dbo.ContactUsSubmissions", "PetOwnerId", "dbo.PetOwners");
            DropForeignKey("dbo.PetMedications", "PetId", "dbo.Pets");
            DropForeignKey("dbo.PetFeedingPlans", "PetId", "dbo.Pets");
            DropForeignKey("dbo.PetEmergencyContacts", "PetId", "dbo.Pets");
            DropForeignKey("dbo.Bookings", "PetId", "dbo.Pets");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BookingStatusHistories", new[] { "EmployeeId" });
            DropIndex("dbo.BookingStatusHistories", new[] { "BookingId" });
            DropIndex("dbo.ContactUsSubmissions", new[] { "PetOwnerId" });
            DropIndex("dbo.PetMedications", new[] { "PetId" });
            DropIndex("dbo.PetFeedingPlans", new[] { "PetId" });
            DropIndex("dbo.PetEmergencyContacts", new[] { "PetId" });
            DropIndex("dbo.Pets", new[] { "PetOwnerId" });
            DropIndex("dbo.Bookings", new[] { "PetId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Employees");
            DropTable("dbo.BookingStatusHistories");
            DropTable("dbo.ContactUsSubmissions");
            DropTable("dbo.PetOwners");
            DropTable("dbo.PetMedications");
            DropTable("dbo.PetFeedingPlans");
            DropTable("dbo.PetEmergencyContacts");
            DropTable("dbo.Pets");
            DropTable("dbo.Bookings");
        }
    }
}
