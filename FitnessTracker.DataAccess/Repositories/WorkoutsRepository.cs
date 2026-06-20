using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Application.WorkoutOrdering;
using FitnessTracker.Shared.DTO;
using System.Linq.Expressions;

namespace FitnessTracker.DataAccess.Repositories
{
    public class WorkoutsRepository
        : IWorkoutsRepository
    {
        FitnessTrackerDbContext _dbContext;
        IWorkoutOrderingApplier _orderingApplier;
        IWorkoutFilterExpressionBuilder _filterExpressionBuilder;

        public WorkoutsRepository(FitnessTrackerDbContext dbContext, 
            IWorkoutOrderingApplier orderingApplier,
            IWorkoutFilterExpressionBuilder filterExpressionBuilder)
        {
            _dbContext = dbContext;
            _orderingApplier = orderingApplier;
            _filterExpressionBuilder = filterExpressionBuilder;
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
            int page = 1,
            int pageSize = 10,
            IEnumerable<WorkoutFilterDTO>? filters = null,
            WorkoutOrderingDTO? ordeing = null)
        {
            IQueryable<Workout> query = _dbContext.Workouts
                .Where(x => x.UserId == userId);

            if (filters is not null)
            {
                var filterExpression = _filterExpressionBuilder.BuildFilterExpression(filters);
                query = query.Where(filterExpression);
            }

            if (ordeing is not null)
            {
                query = _orderingApplier.ApplyOrdering(query, ordeing);
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            if(page <= 0)
            {
                throw new WrongWorkoutPageFormat("Page should be positive integer");
            }

            if (pageSize <= 0)
            {
                throw new WrongWorkoutPageFormat("Page size should be positive integer");
            }

            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize); 

            return await query                
                .ToListAsync();
        }

        public async Task<int> GetTotalCountByUserAsync(string userId,
            IEnumerable<WorkoutFilterDTO>? filters = null)
        {
            IQueryable<Workout> query = _dbContext.Workouts
                .Where(x => x.UserId == userId);

            if (filters is not null)
            {
                var filterExpression = _filterExpressionBuilder.BuildFilterExpression(filters);
                query = query.Where(filterExpression);
            }
            return await query.CountAsync();
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
