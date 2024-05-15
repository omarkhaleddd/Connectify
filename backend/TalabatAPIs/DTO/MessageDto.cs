namespace Talabat.APIs.DTO
{
    public class MessageDto
    {
        public string messageText { get; set; }
        public string userId { get; set; }
        public string displayName { get; set; }
        public DateTime? messageDate { get; set; }
    }
}
