using Microsoft.EntityFrameworkCore;

namespace Scraper.PeoplePhotos.Entities
{
    public class ImagesContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<ImagePerson> ImagesPeople { get; set; }
        public DbSet<Person> People { get; set; }
        
        public ImagesContext(DbContextOptions options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Image>()
                .HasIndex(x => x.Url)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
