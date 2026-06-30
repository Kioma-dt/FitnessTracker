namespace FitnessTracker.Shared.Exceptions
{
    public class ApiException
        : Exception
    {
        public int Code { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public ApiException(int code, string? title, string? details)
            :base(details)
        {
            Code = code;
            Title = title;
            Details = details;
        }
    }
}
