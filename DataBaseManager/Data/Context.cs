using Microsoft.EntityFrameworkCore;

namespace DataBaseManager.Data
{
    public class Context: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
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
                .OnDelete(DeleteBehavior.Cascade);
            
            
            modelBuilder.Entity<Category>(entity =>
            {
                entity
                    .HasMany(c => c.Children)
                    .WithOne(e => e.Parent)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasMany(e => e.IntProperties)
                    .WithOne(x => x.Category)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasMany(e => e.StringProperties)
                    .WithOne(x => x.Category)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasMany(e => e.EnumProperties)
                    .WithOne(x => x.Category)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        
    }
}