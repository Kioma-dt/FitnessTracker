using System.Net;

namespace FitnessTracker.Shared.DTO.Responses
{
    public record StatusResponse(ushort Code, string Message)
    {
        public StatusResponse(HttpStatusCode code, string message)
            : this((ushort)code, message)
        { }
    }
}
