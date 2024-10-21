using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RevenueRec.Models;

namespace RevenueRec.Context
{
    public class s28786Context : DbContext
    {
        private readonly IConfiguration _configuration;

        public s28786Context(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public s28786Context(DbContextOptions<s28786Context> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public s28786Context(DbContextOptions<s28786Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<IndividualClient> IndividualClients { get; set; }
        public virtual DbSet<CompanyClient> CompanyClients { get; set; }

        public virtual DbSet<SoftwareSystem> SoftwareSystems { get; set; }

        public virtual DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<UpFrontContract> UpFrontContracts { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(e => e.IdClient);
            modelBuilder.Entity<Client>()
                .HasDiscriminator<string>("ClientType")
                .HasValue<IndividualClient>("Individual")
                .HasValue<CompanyClient>("Company");
            modelBuilder.Entity<Client>().HasMany(e => e.UpFrontContracts).WithOne(e => e.Client)
                .HasForeignKey(e => e.IdClient).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Client>().HasMany(e => e.Payments).WithOne(e => e.Client)
                .HasForeignKey(e => e.IdClient).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<IndividualClient>().HasIndex(c => c.PESEL).IsUnique();
            modelBuilder.Entity<IndividualClient>().Property<bool>("IsDeleted").HasDefaultValue(false);

            modelBuilder.Entity<CompanyClient>().HasIndex(c => c.KRS).IsUnique();

            modelBuilder.Entity<SoftwareSystem>().HasKey(e => e.IdSoftwareSystem);
            modelBuilder.Entity<SoftwareSystem>().HasMany(e => e.UpFrontContracts).WithOne(e => e.SoftwareSystem)
                .HasForeignKey(e => e.IdSoftwareSystem).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SoftwareSystem>().HasMany(e => e.SoftwareVersions).WithOne(e => e.SoftwareSystem)
                .HasForeignKey(e => e.IdSoftwareSystem).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>().HasKey(e => e.IdCategory);
            modelBuilder.Entity<Category>().HasMany(e => e.SoftwareSystems).WithOne(e => e.Category)
                .HasForeignKey(e => e.IdCategory).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UpFrontContract>().HasKey(e => e.IdUpFrontContract);
            modelBuilder.Entity<UpFrontContract>().HasMany(e => e.Payments).WithOne(e => e.UpFrontContract)
                .HasForeignKey(e => e.IdUpFrontContract).OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<Payment>().HasKey(e => e.IdPayment);

            modelBuilder.Entity<Discount>().HasKey(e => e.IdDiscount);

            modelBuilder.Entity<SoftwareVersion>().HasKey(e => e.IdSoftwareVersion);
            modelBuilder.Entity<SoftwareVersion>().HasMany(e => e.UpFrontContracts).WithOne(e => e.SoftwareVersion)
                .HasForeignKey(e => e.IdSoftwareVersion).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>().HasKey(e => e.IdSubscription);
            modelBuilder.Entity<Subscription>().HasMany(e => e.Payments).WithOne(e => e.Subscription)
                .HasForeignKey(e => e.IdSubscription).OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<Employee>().HasKey(e => e.IdEmployee);
        }
    }
}