using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FitnessTracker.API.Authorization
{
    public class WorkoutOwnerRequirement
        : IAuthorizationRequirement
    {
    }

    public class WorkoutOwnerHandler
        : AuthorizationHandler<WorkoutOwnerRequirement, Workout>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            WorkoutOwnerRequirement requirement,
            Workout resource)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId is not null && resource.UserId == userId.Value) 
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
