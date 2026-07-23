using MiniDeepThought.Configurations;

namespace MiniDeepThought.Domain
{
    public class Job
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public string QuestionText { get; set; } = string.Empty;
        public string AlgorithmKey { get; set; } = string.Empty;
        public string Status { get; set; } = JobStatus.Pending;
        public int Progress { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Result { get; set; } = string.Empty;
    }
}
