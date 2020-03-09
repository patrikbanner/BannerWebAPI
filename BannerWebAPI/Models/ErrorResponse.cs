namespace BannerWebAPI.Models
{
    public class ErrorResponse
    {
        public string[] Errors { get; set; }

        public ErrorResponse(params string[] errors)
        {
            Errors = errors;
        }
    }
}
