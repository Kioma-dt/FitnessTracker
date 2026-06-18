using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class AccessDeniedException
        : ApiException
    {
        public AccessDeniedException(string details)
            : base(403, "Forbiden", details)
        { }
    }
}
