using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class WrongWorkoutOrderingTypeFormatException
        : ApiException
    {
        public WrongWorkoutOrderingTypeFormatException(string details)
            : base(400, "Format of order parameter is wrong", details)
        { }
    }
}
