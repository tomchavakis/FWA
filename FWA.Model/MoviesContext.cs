using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FWA.Model
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : base("name=MoviesContext")
        {
            //Database.SetInitializer<MoviesContext>(new CreateDatabaseIfNotExists<MoviesContext>());
            Database.SetInitializer(new MoviesInitializer());
            this.Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
