using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserRegistration.Models
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users_Table { get; set; }
    }
}
