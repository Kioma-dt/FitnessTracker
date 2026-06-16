using FitnessTracker.DataAccess.Configuration;

namespace FitnessTracker.DataAccess
{
    public class FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
