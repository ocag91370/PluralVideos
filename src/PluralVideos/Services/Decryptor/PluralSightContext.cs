using Microsoft.EntityFrameworkCore;
using PluralVideos.Options;

namespace PluralVideos.Services.Decryptor
{
    public partial class PluralSightContext : DbContext
    {
        private readonly DecryptorOptions options;

        public PluralSightContext(DecryptorOptions options)
        {
            this.options = options;
        }

        public virtual DbSet<Clip> Clip { get; set; }

        public virtual DbSet<ClipTranscript> ClipTranscript { get; set; }

        public virtual DbSet<Course> Course { get; set; }

        public virtual DbSet<Module> Module { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={options.DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Clip>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Clip)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClipTranscript>(entity =>
            {
                entity.HasIndex(e => e.StartTime)
                    .HasName("index_ClipTranscriptStart");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndTime).HasColumnType("integer");

                entity.HasOne(d => d.Clip)
                    .WithMany(p => p.ClipTranscript)
                    .HasForeignKey(d => d.ClipId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired();
            });


            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Module)
                    .HasForeignKey(d => d.CourseName)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
