namespace Talabat.APIs.DTO
{
	public class NotificationDto
	{
		public string content { get; set; }
		public string userId { get; set; }
		public string type { get; set; }
		public DateTime? notificationDate { get; set; }
	}
}
