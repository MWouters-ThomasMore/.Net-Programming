using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data
{
    public class WebAPIDemoContext: DbContext
    {
        public WebAPIDemoContext(DbContextOptions<WebAPIDemoContext> options) : base(options) { }

        public DbSet<Product> Producten {  get; set; }
        public DbSet<Klant> Klanten {  get; set; }
        public DbSet<Bestelling> Bestellingen {  get; set; }
        public DbSet<Orderlijn> Orderlijnen {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling");
            modelBuilder.Entity<Orderlijn>().ToTable("Orderlijn");
            modelBuilder.Entity<Product>().ToTable("Product").Property(p=>p.Prijs).HasColumnType("decimal(18,2)");
        }
    }
}
