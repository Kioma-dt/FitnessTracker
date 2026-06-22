using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configuration
{
    public class WorkoutsConfiguration
       : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Workouts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();

            builder.Property(x => x.CaloriesBurned)
                .IsRequired();

            builder.Property(x => x.WorkoutDate)
                .IsRequired();

            builder.Property(x => x.ProgressPhotos)
                .IsRequired();

            builder.OwnsMany(x => x.Exercises, exercise =>
            {
                exercise.WithOwner()
                    .HasForeignKey("WorkoutId");

                exercise.Property<int>("Id");
                exercise.HasKey("Id");

                exercise.Property(e => e.Name)
                    .IsRequired();

                exercise.OwnsMany(e => e.Sets, set =>
                {
                    set.WithOwner()
                        .HasForeignKey("ExerciseId");

                    set.Property<int>("Id");
                    set.HasKey("Id");

                    set.Property(s => s.Reps)
                        .IsRequired();
                    set.Property(s => s.Weight)
                        .IsRequired();
                });
            });


            builder
                    .ToTable(t =>
                    {
                        t.HasCheckConstraint(
                            "CK_Workout_CaloriesBurned_NonNegative",
                            "\"CaloriesBurned\" >= 0");
                    });

            builder.HasIndex(x => x.UserId);
        }
    }
}
