using FitnessTracker.API.Controllers;
using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO;
using FitnessTracker.Shared.DTO.Queries;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.Exceptions;
using FitnessTracker.API.Cache;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FitnessTracker.Application.Interfaces.Repositories;


namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class GetAllTests
        : WorkoutControllerTestsBase
    {

        [Fact]
        public async Task GetAll_ShouldThrow_WhenUserIdClaimMissing()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);

            var filtersQuery = new WorkoutFiltersQueryDTO();
            var orderingQuery = new WorkoutOrderingQueryDTO();
            var pagesQuery = new WorkoutPagesQueryDTO();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            await Assert.ThrowsAsync<NoInfoInJWTTokenExeption>(() =>
                controller.GetAll(
                    filtersQuery,
                    orderingQuery,
                    pagesQuery));

            workoutsRepositoryMock.Verify(
                x => x.GetTotalCountByUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAll_ShouldReturnPagedResponse()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);

            var userId = "user1";

            SetupUser(userId, controller);

            var filtersQuery = new WorkoutFiltersQueryDTO();

            var orderingQuery = new WorkoutOrderingQueryDTO
            {
                OrderBy = null
            };

            var pagesQuery = new WorkoutPagesQueryDTO
            {
                Page = 1,
                PageSize = 10
            };

            var workouts = new List<Workout>
            {
                new Workout()
            };

            var response = new List<WorkoutResponseDTO>
            {
                new WorkoutResponseDTO
                (
                    "1",
                    "Leg day",
                    WorkoutType.Strength,
                    60,
                    500,
                    new DateTime(2026, 06, 21),
                    new List<ExerciseResponseDTO>
                        {
                            new ExerciseResponseDTO
                            (
                                "Squat",
                                new List<SetResponseDTO>
                                {
                                    new SetResponseDTO(10, 100),
                                    new SetResponseDTO(8, 120)
                                }
                            )
                        },
                    new List<string> { "photo1.jpg" }
                )
            };

            workoutsRepositoryMock
                .Setup(x => x.GetTotalCountByUserAsync(
                    userId,
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>()))
                .ReturnsAsync(1);

            workoutsRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(
                    userId,
                    1,
                    10,
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>(),
                    null))
                .ReturnsAsync(workouts);

            mapperMock
                .Setup(x => x.Map<IEnumerable<WorkoutResponseDTO>>(workouts))
                .Returns(response);

            var result = await controller.GetAll(
                filtersQuery,
                orderingQuery,
                pagesQuery);

            Assert.IsType<OkObjectResult>(result);
            var or = result as OkObjectResult;
            Assert.IsType<PagedResponseDTO<WorkoutResponseDTO>>(or?.Value);
            var resp = or.Value as PagedResponseDTO<WorkoutResponseDTO>;

            Assert.Single(resp?.Items ?? new List<WorkoutResponseDTO>());

            Assert.Equal(1, resp?.TotalRecords);

            workoutsRepositoryMock.Verify(
                    x => x.GetAllByUserIdAsync(
                        userId,
                        1,
                        10,
                        It.IsAny<IEnumerable<WorkoutFilterDTO>>(),
                        null),
                    Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldPassOrderingToRepository_WhenOrderingSpecified()
        {
            var workoutsRepositoryMock = new Mock<IWorkoutsRepository>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mapperMock = new Mock<IMapper>();
            var eTagGeneratorMock = new Mock<IETagGenerator>();

            var controller = new WorkoutController(
                workoutsRepositoryMock.Object,
                authorizationServiceMock.Object,
                eTagGeneratorMock.Object,
                mapperMock.Object);

            var userId = "user1";

            SetupUser(userId, controller);

            var orderingQuery = new WorkoutOrderingQueryDTO
            {
                OrderBy = WorkoutOrderingType.Date,
                Descending = true
            };

            var pagesQuery = new WorkoutPagesQueryDTO
            {
                Page = 1,
                PageSize = 10
            };

            workoutsRepositoryMock
                .Setup(x => x.GetTotalCountByUserAsync(
                    userId,
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>()))
                .ReturnsAsync(0);

            workoutsRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(
                    userId,
                    1,
                    10,
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>(),
                    It.Is<WorkoutOrderingDTO>(o =>
                        o.OrderBy == WorkoutOrderingType.Date &&
                        o.Descending)))
                .ReturnsAsync(new List<Workout>());

            mapperMock
                .Setup(x => x.Map<IEnumerable<WorkoutResponseDTO>>(It.IsAny<IEnumerable<Workout>>()))
                .Returns(new List<WorkoutResponseDTO>());


            await controller.GetAll(
                new WorkoutFiltersQueryDTO(),
                orderingQuery,
                pagesQuery);


            workoutsRepositoryMock.Verify(x =>
                x.GetAllByUserIdAsync(
                    userId,
                    1,
                    10,
                    It.IsAny<IEnumerable<WorkoutFilterDTO>>(),
                    It.Is<WorkoutOrderingDTO>(o =>
                        o.OrderBy == WorkoutOrderingType.Date &&
                        o.Descending)),
                Times.Once);
        }
    }
}
