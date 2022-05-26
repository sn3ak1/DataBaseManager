using Microsoft.EntityFrameworkCore;

namespace DataBaseManager.Data
{
    public class Context: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<IntProperty> IntProperties { get; set; }
        public DbSet<StringProperty> StringProperties { get; set; }
        public DbSet<EnumProperty> EnumProperties { get; set; }
        public DbSet<EnumFlag> EnumFlags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                "server=remotemysql.com;database=HEjS6WcHwB;user=HEjS6WcHwB;password=dpzDmZUHak");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany<Category>(c => c.Children)
                .WithOne(e => e.Parent)
                .OnDelete(DeleteBehavior.Cascade );

            // modelBuilder.Entity<IntProperty>()
            //     .ToTable("IntProperties")
            //     .HasKey(x => x.Id);
            //
            // modelBuilder.Entity<StringProperty>()
            //     .ToTable("StringProperties")
            //     .HasKey(x => x.Id);
            //
            // modelBuilder.Entity<EnumProperty>()
            //     .ToTable("EnumProperties")
            //     .HasKey(x => x.Id);
            //
            // modelBuilder.Entity<EnumFlag>()
            //     .ToTable("EnumFlags")
            //     .HasKey(x => x.Id);
        }
        
    }
}