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

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     
        //     modelBuilder.Entity<Object>(entity =>
        //     {
        //         entity.HasKey(e => e.Id);
        //         entity.Property(e => e.Name).IsRequired();
        //     });
        //
        //     modelBuilder.Entity<IntProperty>(entity =>
        //     {
        //         entity.HasKey(e => e.Id);
        //         entity.Property(e => e.Name).IsRequired();
        //     });
        //     
        //     modelBuilder.Entity<StringProperty>(entity =>
        //     {
        //         entity.HasKey(e => e.Id);
        //         entity.Property(e => e.Name).IsRequired();
        //     });
        //     
        //     modelBuilder.Entity<EnumProperty>(entity =>
        //     {
        //         entity.HasKey(e => e.Id);
        //         entity.Property(e => e.Name).IsRequired();
        //         entity.HasMany(e => e.Flags);
        //     });
        // }
        
    }
}