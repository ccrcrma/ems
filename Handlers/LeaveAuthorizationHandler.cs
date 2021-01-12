using System.Security.Claims;
using System.Threading.Tasks;
using ems.Models;
using Microsoft.AspNetCore.Authorization;
using static ems.Models.Reply;

namespace ems.Handlers
{
    public class LeaveAuthorizationHandler : AuthorizationHandler<OwnsLeaveRequirement, Leave>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OwnsLeaveRequirement requirement,
            Leave resource)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (resource.OwnerId == userId && requirement.Status == ReplyStatus.Pending)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class OwnsLeaveRequirement : IAuthorizationRequirement
    {
        public OwnsLeaveRequirement(ReplyStatus status)
        {
            Status = status;
        }

        public ReplyStatus Status { get; }
    }
}