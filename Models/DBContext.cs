using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace la_brisa.Models
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)

           : base(options)

        {

            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}
