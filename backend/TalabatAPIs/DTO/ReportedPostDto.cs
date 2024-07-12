namespace Talabat.APIs.DTO
{
    public class ReportedPostDto
    {

        public int Id { get; set; }

        public int PostId { get; set; }
        public PostDto Post { get ; set; }

        public string Status { get; set; }

    }
}
