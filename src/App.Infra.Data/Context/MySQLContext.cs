using App.Domain.Models;
using App.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;


namespace App.Infra.Data.Context
{
    public class MySQLContext : DbContext
    {

        public MySQLContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new VideosMap());
        }

        public DbSet<VideoBD> Videos { get; set; }
    }
}
