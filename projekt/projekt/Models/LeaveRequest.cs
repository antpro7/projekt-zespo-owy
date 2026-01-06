namespace projekt.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // Te nazwy musz¹ byæ identyczne jak w SQL
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = "Urlop wypoczynkowy";
        public string Status { get; set; } = "Pending";
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // DODAJ TÊ LINIÊ, bo stworzyliœmy j¹ w SQL
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Relacja do u¿ytkownika
        public User? User { get; set; }
    }
}