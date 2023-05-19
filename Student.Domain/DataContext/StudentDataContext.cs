using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student.Domain.Model;
using Microsoft.EntityFrameworkCore.Design;

namespace Student.Domain.DataContext
{
    public class StudentDataContext : DbContext
    {
        public StudentDataContext(DbContextOptions<StudentDataContext> options) : base(options)
        {
        }
        public DbSet<Model.Student> students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-MRDBE1E;Trusted_Connection=True;Initial Catalog=StudentDB;TrustServerCertificate=True;multipleactiveresultsets=true;");
        }
    }

    public class StudentDataContextFactory : IDesignTimeDbContextFactory<StudentDataContext>
    {
        public StudentDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentDataContext>();
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));

            return new StudentDataContext(optionsBuilder.Options);
        }
    }
}

//EntityFrameworkCore\Add-Migration init -Context Student.Domain.DataContext.StudentDataContext
//Update-Database 