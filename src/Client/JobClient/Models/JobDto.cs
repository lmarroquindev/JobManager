namespace JobClient.Models
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string JobType { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

}
