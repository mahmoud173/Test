using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
               new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
               new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
           );

            builder.Entity<Department>().HasData(
                 new Department { Id = 1, Name = "HR" },
                 new Department { Id = 2, Name = "Sales" },
                 new Department { Id = 3, Name = "Productes" }
                 );

            builder.Entity<Employee>().HasData(
              new Employee { Id = 1, Name = "Ali", Salary = 1000, DepartmentId = 1 },
              new Employee { Id = 2, Name = "Ahmed", Salary = 2000, DepartmentId = 2 },
              new Employee { Id = 3, Name = "Mahmoud", Salary = 500, DepartmentId = 3 }
              );
        }
    }
}
