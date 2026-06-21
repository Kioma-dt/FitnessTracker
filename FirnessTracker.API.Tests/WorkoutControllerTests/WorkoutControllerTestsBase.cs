using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirnessTracker.API.Tests.WorkoutControllerTests
{
    public class WorkoutControllerTestsBase
    {
        protected void SetupUser(string userId, ControllerBase controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                        new Claim(ClaimTypes.NameIdentifier, userId)
                            }))
                }
            };
        }
    }
}
