namespace FitnessTracker.API.Cache
{
    public static class ETagHelper
    {
        public static void SetETag(HttpResponse response, string etag)
        {
            response.Headers.ETag = $"\"{etag}\"";
            response.Headers.CacheControl = "private, no-cache";
        }

        public static bool IsNotModified(HttpRequest request, string currentETag)
        {
            var clientETag = request.Headers.IfNoneMatch.ToString().Trim('"');
            return !string.IsNullOrWhiteSpace(clientETag) && clientETag == currentETag;
        }
         
    }
}
