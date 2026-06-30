using FitnessTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Inrastructure.DataSellection.Filterring.WorkoutFilters
{
    public interface IWorkoutFilter
    {
        WorkoutFilterType FilterType { get; }
        Expression BuildExpression(ParameterExpression parameter, string value);
    }

    public class FromDateWorkoutFilter 
        : IWorkoutFilter
    {
        public WorkoutFilterType FilterType => WorkoutFilterType.FromDate;

        public Expression BuildExpression(ParameterExpression parameter, string value)
        {
            try
            {
                DateTime parsedValue = DateTime.Parse(value).ToUniversalTime();
                var property = Expression.Property(
                    parameter,
                    nameof(Workout.WorkoutDate));
                var constant = Expression.Constant(parsedValue);
                return Expression.GreaterThanOrEqual(property, constant);
            }
            catch (FormatException)
            {
                throw new WrongFilterValueFormatException("FromDate value should be date!");
            }
        }
    }

    public class ToDateWorkoutFilter 
        : IWorkoutFilter
    {
        public WorkoutFilterType FilterType => WorkoutFilterType.ToDate;

        public Expression BuildExpression(ParameterExpression parameter, string value)
        {
            try
            {
                DateTime parsedValue = DateTime.Parse(value).ToUniversalTime();
                var property = Expression.Property(
                    parameter,
                    nameof(Workout.WorkoutDate));
                var constant = Expression.Constant(parsedValue);
                return Expression.LessThanOrEqual(property, constant);
            }
            catch (FormatException)
            {
                throw new WrongFilterValueFormatException("ToDate value should be date!");
            }
        }
    }

    public class MinDurationWorkoutFilter 
        : IWorkoutFilter
    {
        public WorkoutFilterType FilterType => WorkoutFilterType.MinDuration;

        public Expression BuildExpression(ParameterExpression parameter, string value)
        {
            try
            {
                TimeSpan parsedValue = TimeSpan.Parse(value);
                var property = Expression.Property(
                    parameter,
                    nameof(Workout.Duration));
                var constant = Expression.Constant(parsedValue);
                return Expression.GreaterThanOrEqual(property, constant);
            }
            catch (FormatException)
            {
                throw new WrongFilterValueFormatException("MinDuartion value should be non negative integer minutes!");
            }
        }
    }

    public class MaxDurationWorkoutFilter
        : IWorkoutFilter
    {
        public WorkoutFilterType FilterType => WorkoutFilterType.MaxDuration;

        public Expression BuildExpression(ParameterExpression parameter, string value)
        {
            try
            {
                TimeSpan parsedValue = TimeSpan.Parse(value);
                var property = Expression.Property(
                    parameter,
                    nameof(Workout.Duration));
                var constant = Expression.Constant(parsedValue);
                return Expression.LessThanOrEqual(property, constant);
            }
            catch (FormatException)
            {
                throw new WrongFilterValueFormatException("MaxDuartion value should be non negative integer minutes!");
            }
        }
    }

    public class TypeWorkoutFilter 
        : IWorkoutFilter
    {
        public WorkoutFilterType FilterType => WorkoutFilterType.WorkoutType;

        public Expression BuildExpression(ParameterExpression parameter, string value)
        {
            try
            {
                WorkoutType parsedValue = Enum.Parse<WorkoutType>(value, true);
                var property = Expression.Property(
                    parameter,
                    nameof(Workout.Type));
                var constant = Expression.Constant(parsedValue);
                return Expression.Equal(property, constant);
            }
            catch (ArgumentException)
            {
                throw new WrongFilterValueFormatException("WorkoutType value not found!");
            }
        }
    }
}
