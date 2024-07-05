namespace dashboard.Entities
{
    public class Report
    {
        public int PostId { get; set; }
        public string ReportedBy { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
