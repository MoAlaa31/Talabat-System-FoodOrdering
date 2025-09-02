
namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode); 
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request: The server could not understand the request due to invalid syntax.",
                401 => "Unauthorized: You must authenticate before accessing this resource.",
                404 => "Not Found: The requested resource could not be found on the server.",
                500 => "Internal Server Error: An unexpected error occurred. Please try again later.",
                _ => null
            };
        }
    }
}
