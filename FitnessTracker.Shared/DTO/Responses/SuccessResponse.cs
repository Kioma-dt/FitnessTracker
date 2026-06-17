using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FitnessTracker.Shared.DTO.Responses
{
    public record SuccessResponse<T>(ushort Code, string Message, T Data)
    {
        public SuccessResponse(HttpStatusCode code, string message, T data)
            : this((ushort)code, message, data)
        { }
    }
}
