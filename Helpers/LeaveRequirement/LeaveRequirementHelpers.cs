using ems.Handlers;
using ems.Models;

namespace ems.Helpers.LeaveRequirement
{
    public static class LeaveRequirementHelpers
    {
        public static OwnsLeaveRequirement GetRequirementForLeave(Leave leave)
        {
            var status = leave.Reply?.Status ?? Reply.ReplyStatus.Pending;
            var requirement = new OwnsLeaveRequirement(status);
            return requirement;
        }

    }
}