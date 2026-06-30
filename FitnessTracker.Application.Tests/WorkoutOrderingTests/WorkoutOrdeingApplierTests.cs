using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FitnessTracker.Application.Tests.WorkoutOrderingTests
{
    public class WorkoutOrdeingApplierTests
    {
        [Fact]
        public void ApplyOrdering_ShouldOrderByAscending_WhenDescendingIsFalseOrNull()
        {
            var orderMock = new Mock<IWorkoutOrder>();
            orderMock.Setup(x => x.WorkoutOrderingType)
                .Returns(WorkoutOrderingType.Date);
            orderMock.Setup(x => x.BuildExpression(It.IsAny<ParameterExpression>()))
                .Returns((ParameterExpression p) =>
                    Expression.Property(p, nameof(Workout.WorkoutDate)));

            var applier = new WorkoutOrderingApplier([orderMock.Object]);

            var query = GetWorkouts().AsQueryable();

            var dto = new WorkoutOrderingDTO(
                WorkoutOrderingType.Date,
                false);

            var result = applier.ApplyOrdering(query, dto).ToList();

            Assert.Equal("1", result[0].Id);
            Assert.Equal("3", result[1].Id);
            Assert.Equal("2", result[2].Id);
        }


        [Fact]
        public void ApplyOrdering_ShouldOrderByDescending_WhenDescendingIsTrue()
        {
            var orderMock = new Mock<IWorkoutOrder>();
            orderMock.Setup(x => x.WorkoutOrderingType)
                .Returns(WorkoutOrderingType.Date);
            orderMock.Setup(x => x.BuildExpression(It.IsAny<ParameterExpression>()))
                .Returns((ParameterExpression p) =>
                    Expression.Property(p, nameof(Workout.WorkoutDate)));

            var applier = new WorkoutOrderingApplier([orderMock.Object]);

            var query = GetWorkouts().AsQueryable();

            var dto = new WorkoutOrderingDTO(
                WorkoutOrderingType.Date,
                true);

            var result = applier.ApplyOrdering(query, dto).ToList();

            Assert.Equal("2", result[0].Id);
            Assert.Equal("3", result[1].Id);
            Assert.Equal("1", result[2].Id);
        }


        [Fact]
        public void ApplyOrdering_ShouldOrderById_WhenOrderParamsAreTheSame()
        {
            var orderMock = new Mock<IWorkoutOrder>();

            orderMock.Setup(x => x.WorkoutOrderingType)
                .Returns(WorkoutOrderingType.Date);

            orderMock.Setup(x => x.BuildExpression(It.IsAny<ParameterExpression>()))
                .Returns((ParameterExpression p) =>
                    Expression.Property(p, nameof(Workout.WorkoutDate)));

            var applier = new WorkoutOrderingApplier([orderMock.Object]);

            var workouts = GetWorkouts();
            workouts.Add(new Workout { Id = "4", WorkoutDate = new DateTime(2026, 06, 2) });
            var query = workouts.AsQueryable();

            var dto = new WorkoutOrderingDTO(
                WorkoutOrderingType.Date,
                false);


            var result = applier.ApplyOrdering(query, dto).ToList();

            Assert.Equal("1", result[0].Id);
            Assert.Equal("3", result[1].Id);
            Assert.Equal("4", result[2].Id);
            Assert.Equal("2", result[3].Id);
        }

        [Fact]
        public void ApplyOrdering_ShouldThrow_WhenOrderingTypeNotImplemented()
        {
            var orderMock = new Mock<IWorkoutOrder>();

            orderMock.Setup(x => x.WorkoutOrderingType)
                .Returns(WorkoutOrderingType.CaloriesBurned);
            orderMock.Setup(x => x.BuildExpression(It.IsAny<ParameterExpression>()))
                .Returns((ParameterExpression p) =>
                    Expression.Property(p, nameof(Workout.CaloriesBurned)));
            var applier = new WorkoutOrderingApplier([orderMock.Object]);

            var query = GetWorkouts().AsQueryable();

            var dto = new WorkoutOrderingDTO(
                WorkoutOrderingType.Date,
                false);

            Assert.Throws<NotImplementedFunctionalityException>(() =>
                applier.ApplyOrdering(query, dto));
        }

        private List<Workout> GetWorkouts() =>
        [
            new Workout { Id = "1", WorkoutDate = new DateTime(2026, 06, 1) },
            new Workout { Id = "2", WorkoutDate = new DateTime(2026, 06, 3) },
            new Workout { Id = "3", WorkoutDate = new DateTime(2026, 06, 2) },
        ];

    }
}
