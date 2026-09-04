using FitnessTracker.DataAccess.Configuration;
using FitnessTracker.DataAccess.Interceptors;

namespace FitnessTracker.DataAccess
{
    public class FitnessTrackerDbContext
        : DbContext
    {
        public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options)
            :base(options)
        {}

        public DbSet<User> Users {  get; set; }
        public DbSet<Workout> Workouts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder config)
        {
            config.AddInterceptors(new AuditingSaveChangesInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
