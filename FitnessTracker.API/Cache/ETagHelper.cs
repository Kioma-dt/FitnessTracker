using FitnessTracker.Shared.Exceptions.PreconditionFailed;
using FitnessTracker.Shared.Exceptions.PreconditionRequired;

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

        public static void ValidateIfMatch(HttpRequest request, string currentETag)
        {
            var ifMatch = request.Headers.IfMatch.ToString()?.Trim('"');
            
            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                throw new NoIfMatchException("If-Match header is required for this operation");
            }

            if (ifMatch != "*" && ifMatch != currentETag)
            {
                throw new ResourceHasBeenModifiedException("Fetch the latest version of this resource");
            }
        }
    }
}
