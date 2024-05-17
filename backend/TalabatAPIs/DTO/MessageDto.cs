namespace Talabat.APIs.DTO
{
    public class MessageDto
    {
        public string messageText { get; set; }
        public string senderId { get; set; }
        public string senderName { get; set; }
        public string recieverId { get; set; }
        public string recieverName { get; set; }
        public DateTime? messageDate { get; set; }
    }
}
