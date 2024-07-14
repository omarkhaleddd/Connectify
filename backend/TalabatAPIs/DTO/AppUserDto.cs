namespace Talabat.APIs.DTO
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }
        public IFormFile ProfileimageFile { get; set; }
        public string CoverImageUrl { get; set; }
        public IFormFile CoverImageFile { get; set; }

    }
}
