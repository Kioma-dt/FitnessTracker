using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FitnessTracker.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WorkoutFilterType
    {
        FromDate,
        ToDate,
        MinDuration,
        MaxDuration,
        WorkoutType
    }
}
