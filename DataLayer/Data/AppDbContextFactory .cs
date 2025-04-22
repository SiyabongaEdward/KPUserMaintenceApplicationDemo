using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=JARVIS\\SQLEXPRESS;Database=UserGroupDb;Trusted_Connection=True;TrustServerCertificate=True;"); // Replace with your actual connection string

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
