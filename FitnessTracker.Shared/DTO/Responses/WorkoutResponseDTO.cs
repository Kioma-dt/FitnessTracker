using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.DTO.Responses
{
    public record WorkoutResponseDTO
    (
        string Id,
        string Title,
        WorkoutType Type,
        int DurationInMinutes,
        int CaloriesBurned,
        DateTime WorkoutDate,
        List<ExerciseResponseDTO> Exercises
        );

    public record ExerciseResponseDTO
    (
        string Name,
        List<SetResponseDTO> Sets
        );

    public record SetResponseDTO
    (
        double Weight,
        int Reps
        );
}
