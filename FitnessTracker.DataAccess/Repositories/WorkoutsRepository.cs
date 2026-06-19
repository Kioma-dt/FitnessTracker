using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.Shared.DTO;
using System.Linq.Expressions;

namespace FitnessTracker.DataAccess.Repositories
{
    public class WorkoutsRepository
        : IWorkoutsRepository
    {
        FitnessTrackerDbContext _dbContext;
        IWorkoutOrderingApllier _orderingApplier;

        public WorkoutsRepository(FitnessTrackerDbContext dbContext, IWorkoutOrderingApllier orderingApplier)
        {
            _dbContext = dbContext;
            _orderingApplier = orderingApplier;
        }

        public async Task AddAsync(Workout workout)
        {
            if (workout.Id is not null)
            {
                var dbWorkout = await _dbContext.Workouts
                    .FirstOrDefaultAsync(x => x.Id == workout.Id);

                if (dbWorkout is not null)
                {
                    throw new EntityAlreadyExistsException($"Workout with id: {workout.Id} alredy exists!");
                }
            }

            await _dbContext.Workouts.AddAsync(workout);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddExerciseAsync(string id, Exercise exercise)
        {
            var dbWorkout = await _dbContext.Workouts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbWorkout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            dbWorkout.AddExercise(exercise);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPhotoAsync(string id, string photo)
        {
            var dbWorkout = await _dbContext.Workouts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbWorkout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            dbWorkout.AddPhoto(photo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var dbWorkout = await _dbContext.Workouts
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (dbWorkout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            _dbContext.Workouts
                .Remove(dbWorkout);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId,
            Expression<Func<Workout, bool>>? filter = null,
            string? orderBy = null,
            bool? descending = null)
        {
            IQueryable<Workout> query = _dbContext.Workouts
                .Where(x => x.UserId == userId);

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            query = _orderingApplier.ApplyOrdering(query, new WorkoutOrderingDTO(orderBy, descending));

            return await query                
                .ToListAsync();
        }

        public async Task<Workout?> GetByIdAsync(string id)
        {
            return await _dbContext.Workouts
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Workout> UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO)
        {
            var dbWorkout = await _dbContext.Workouts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbWorkout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            dbWorkout.Title = workoutUpdateDTO.Title ?? dbWorkout.Title;
            dbWorkout.Duration = workoutUpdateDTO.Duration ?? dbWorkout.Duration;
            dbWorkout.Type = workoutUpdateDTO.Type ?? dbWorkout.Type;
            dbWorkout.WorkoutDate = workoutUpdateDTO.WorkoutDate?.ToUniversalTime() ?? dbWorkout.WorkoutDate;

            if (workoutUpdateDTO.Exercises is not null)
            {
                dbWorkout.Exercises.Clear();
                dbWorkout.Exercises.AddRange(workoutUpdateDTO.Exercises
                    .Select(x => 
                        new Exercise(
                            x.Name,
                            x?.Sets?
                                .Select(s => 
                                new Set(
                                    s.Reps, 
                                    s.Weight)
                                ).ToList())
                        ).ToList());
            }

            if(workoutUpdateDTO.ProgressPhotos is not null)
            {
                dbWorkout.ProgressPhotos.Clear();
                dbWorkout.ProgressPhotos.AddRange(workoutUpdateDTO.ProgressPhotos);
            }

            await _dbContext.SaveChangesAsync();
            return dbWorkout;
        }
    }
}
