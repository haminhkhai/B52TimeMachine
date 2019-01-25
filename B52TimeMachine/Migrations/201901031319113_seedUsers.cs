namespace B52TimeMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'866e8488-9a82-47fe-bad2-5bb5bad8a9e2', NULL, 0, N'AOi08BaY5ux/X2RJX7Cj1v0FLeydVYq/2qhLMH1cwl4ks9DhtcJU86b5OXmo1IOTlQ==', N'5d73873e-885a-465e-a0f1-53385b660b79', NULL, 0, 0, NULL, 0, 0, N'admin')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'bea87a8f-b108-4cd5-b244-d3d7f3147363', NULL, 0, N'AAbPFiAa2+QYfV1bdcmWTbwU0QiTjoZZw3djjDcMezQJCxfaHFdmM4/q3S6L3T1hVw==', N'32d020c6-80ee-4c5a-aa0f-90a256169f03', NULL, 0, 0, NULL, 0, 0, N'userone')
                
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'44fa05e5-8c92-479e-9fa0-9f078a36c64d', N'Admin')
                
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'866e8488-9a82-47fe-bad2-5bb5bad8a9e2', N'44fa05e5-8c92-479e-9fa0-9f078a36c64d')
            ");

            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS1', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS2', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS3', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS4', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS5', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS6', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS7', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS8', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS9', 18000, 1, true)");
            //Sql("Insert into Ps (Name, Price, IsAvailable, IsVisible) values ('PS10', 18000, 1, true)");
        }
        
        public override void Down()
        {
        }
    }
}
