using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Drive.Data.Seed;

namespace Drive.Data.Entities
{
    public class DriveDbContext : DbContext
    {
        public DriveDbContext(DbContextOptions<DriveDbContext> options) : base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Entities.Models.File> Files { get; set; }
        public DbSet<SharedItem> SharedItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();


            modelBuilder.Entity<Folder>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.Owner);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.Subfolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Entities.Models.File>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Entities.Models.File>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Models.File>()
                .HasOne(o => o.Owner);


            modelBuilder.Entity<SharedItem>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SharedItem>()
                .HasOne(s => s.SharedBy)
                .WithMany()
                .HasForeignKey(s => s.SharedById)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SharedItem>()
                .HasOne(s => s.SharedWith)
                .WithMany()
                .HasForeignKey(s => s.SharedWithId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.File)
                .WithMany()
                .HasForeignKey(c => c.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            DatabaseSeeder.InitialSeed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DriveDbContextFactory : IDesignTimeDbContextFactory<DriveDbContext>
    {
        public DriveDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=127.0.0.1;Port=5432;Database=Drive;User Id=postgres;Password=rootuser";

            var options = new DbContextOptionsBuilder<DriveDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new DriveDbContext(options);
        }
    }
}
