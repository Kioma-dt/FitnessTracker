using Microsoft.EntityFrameworkCore;
using Moq;

namespace FitnessTracker.DataAccess.Tests.WorkoutsRepositoryTests
{
    public class WorkoutsRepositoryTestsBase
    {
        protected FitnessTrackerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitnessTrackerDbContext(options);
        }

        protected readonly Mock<IWorkoutOrderingApplier> _orderingApplierMock;
        protected readonly Mock<IWorkoutFilterExpressionBuilder> _filterBuilderMock;

        public WorkoutsRepositoryTestsBase()
        {
            _orderingApplierMock = new Mock<IWorkoutOrderingApplier>();
            _filterBuilderMock = new Mock<IWorkoutFilterExpressionBuilder>();
        }
    }
}
