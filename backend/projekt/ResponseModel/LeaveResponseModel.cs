using projekt.Models;

namespace projekt.ResponseModel
{
    public class LeaveResponseModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDataResponseModel? User { get; set; }
        public LeaveResponseModel(LeaveRequest leaveRequest)
        {
            this.Id = leaveRequest.Id;
            this.StartDate = leaveRequest.StartDate;
            this.EndDate = leaveRequest.EndDate;
            this.LeaveType = leaveRequest.LeaveType;
            this.Status = leaveRequest.Status;
            this.Comment = leaveRequest.Comment;
            this.CreatedAt = leaveRequest.CreatedAt;
            this.UpdatedAt = leaveRequest.UpdatedAt;
            if (leaveRequest.User != null)
            {
                this.User = new UserDataResponseModel(leaveRequest.User);
            }
        }
    }
}
