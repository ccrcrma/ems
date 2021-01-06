namespace ems.Models
{
    public class Reply
    {
        public enum ReplyStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
        public int Id { get; set; }
        public ReplyStatus Status { get; set; }
        public int LeaveId { get; set; }
        public string Description { get; set; }






    }
}