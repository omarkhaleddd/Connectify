namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode,string? message=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStausCode(StatusCode);
        }

        private string? GetDefaultMessageForStausCode(int? code)
        {
            return code switch
            {
                400 => "BadRequest",
                401 => "You are not authorized",
                404 => "resource not found",
                500 => "internal server error",
                _ => null




            };
        }
    }
}
