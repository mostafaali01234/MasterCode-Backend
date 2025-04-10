using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace DataAccess.Data
{
    public class RolesConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        private const string adminId = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string employeeId = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
        private const string customerId = "01B168FE-810B-432D-9010-233BA0B380E9";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {

            builder.HasData(
                    new IdentityRole
                    {
                        Id = adminId,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole
                    {
                        Id = employeeId,
                        Name = "Employee",
                        NormalizedName = "EMPLOYEE"
                    },
                    new IdentityRole
                    {
                        Id = customerId,
                        Name = "Customer",
                        NormalizedName = "CUSTOMER"
                    }
                );
        }
    }
    public class JobsConfigurations : IEntityTypeConfiguration<Job>
    {
        private const int ManagementId = 1;
        private const int ProgrammerId = 2;
        private const int TechId = 3;
        private const int MarketingId = 4;
        private const int SalesId = 5;

        public void Configure(EntityTypeBuilder<Job> builder)
        {

            builder.HasData(
                    new Job
                    {
                        Id = ManagementId,
                        Name = "ادارة",
                        CreatedTime = DateTime.UtcNow,
                        Description = ""
                    },
                    new Job
                    {
                        Id = ProgrammerId,
                        Name = "برمجة",
                        CreatedTime = DateTime.UtcNow,
                        Description = ""
                    },
                    new Job
                    {
                        Id = TechId,
                        Name = "تركيبات",
                        CreatedTime = DateTime.UtcNow,
                        Description = ""
                    }, new Job
                    {
                        Id = MarketingId,
                        Name = "تسويق",
                        CreatedTime = DateTime.UtcNow,
                        Description = ""
                    }, new Job
                    {
                        Id = SalesId,
                        Name = "مبيعات",
                        CreatedTime = DateTime.UtcNow,
                        Description = ""
                    }
                );
        }
    }
    public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private const string adminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var admin = new ApplicationUser
            {
                Id = adminId,
                UserName = "masteradmin",
                NormalizedUserName = "MASTERADMIN",
                Name = "MasterAdmin",
                Email = "Admin@Admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                PhoneNumber = "01153284612",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                JobId = 1
                
            };

            admin.PasswordHash = PassGenerate(admin);

            builder.HasData(admin);
        }

        public string PassGenerate(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "password");
        }
    }
    public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private const string adminUserId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        private const string adminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            IdentityUserRole<string> iur = new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            };

            builder.HasData(iur);
        }
    }

}
