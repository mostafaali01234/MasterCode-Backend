using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<MoneySafe> MoneySafes { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<OrderActivation> OrderActivations { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "DesktopApp", DisplayOrder = 1, Description = "تطبيق سطح المكتب", CreatedTime = DateTime.Now },
                new Category { Id = 2, Name = "WebApp", DisplayOrder = 2, Description = "تطبيق ويب", CreatedTime = DateTime.Now },
                new Category { Id = 3, Name = "MobileApp", DisplayOrder = 3, Description = "تطبيق موبايل", CreatedTime = DateTime.Now }
                );

            //modelBuilder.Entity<IdentityRole>().HasData(
            //    new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
            //    new IdentityRole { Name = "Customer", NormalizedName = "Customer".ToUpper() },
            //    new IdentityRole { Name = "Employee", NormalizedName = "Employee".ToUpper() }
            //    );
            
            //modelBuilder.Entity<ApplicationUser>().HasData(
            //    new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
            //    new IdentityRole { Name = "Customer", NormalizedName = "Customer".ToUpper() },
            //    new IdentityRole { Name = "Employee", NormalizedName = "Employee".ToUpper() }
            //    );

            modelBuilder.ApplyConfiguration(new JobsConfigurations());
            modelBuilder.ApplyConfiguration(new RolesConfigurations());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new UsersWithRolesConfig());

        }
    }
}
