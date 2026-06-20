using FitnessTracker.Shared.DTO.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.Mappers
{
    [Mapper]
    public interface IWorkoutMapper
    {
        WorkoutResponseDTO MapTo(Workout workout);
        WorkoutResponseDTO MapTo(Workout workout, WorkoutResponseDTO workoutResponseDTO);
        IEnumerable<WorkoutResponseDTO> MapTo(IEnumerable<Workout> workouts)
        => workouts.Select(MapTo);

        ExerciseResponseDTO MapTo(Exercise exercise);
        ExerciseResponseDTO MapTo(Exercise exercise, ExerciseResponseDTO exerciseResponseDTO);

        SetResponseDTO MapTo(Set set);
        SetResponseDTO MapTo(Set set, SetResponseDTO setResponseDTO);
    }
}
