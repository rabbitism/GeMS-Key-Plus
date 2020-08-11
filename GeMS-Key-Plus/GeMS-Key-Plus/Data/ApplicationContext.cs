using GeMS_Key_Plus.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeMS_Key_Plus.Data
{
    public class ApplicationContext:DbContext
    {
        public DbSet<LinkButton> Buttons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=./ApplicationData.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LinkButton>().HasKey(a => a.Id);
        }
    }
}
