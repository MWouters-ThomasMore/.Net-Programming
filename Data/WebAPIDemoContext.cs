namespace WebAPIDemo.Data
{
    public class WebAPIDemoContext : DbContext
    {
        public WebAPIDemoContext(DbContextOptions<WebAPIDemoContext> options) : base(options)
        {
        }

        public DbSet<Product> Producten { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Orderlijn> Orderlijnen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling");
            modelBuilder.Entity<Orderlijn>().ToTable("Orderlijn");
            modelBuilder.Entity<Product>().ToTable("Product").Property(p => p.Prijs).HasColumnType("decimal(18,2)");

            // Fluent Api -> Declareer de structuur (relaties, types etc.) hier.

            // One to many
            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.Klant)
                .WithMany(x => x.Bestellingen)
                .HasForeignKey(x => x.KlantId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Many to many
            modelBuilder.Entity<Orderlijn>()
                .HasOne(x => x.Bestelling)
                .WithMany(x => x.Orderlijnen)
                .HasForeignKey(x => x.BestellingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Orderlijn>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Orderlijnen)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}