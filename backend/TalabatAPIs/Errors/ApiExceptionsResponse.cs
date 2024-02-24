namespace Talabat.APIs.Errors
{
    public class ApiExceptionsResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionsResponse(int statuscode,string? message=null,string? details=null ):base(statuscode, message)
        {
            Details = details;
        }
    }

}
