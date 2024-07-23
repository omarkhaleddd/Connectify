namespace Talabat.APIs.DTO
{
    public class GroupMessageDto
    {
        public string messageText { get; set; }
        public string senderId { get; set; }
        public string senderName { get; set; }
        public string? recieverId { get; set; }
        public string? recieverName { get; set; }
        public string groupName { get; set; }
        public DateTime? messageDate { get; set; }
    }
}
