using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class WrongWorkoutPageFormat
        : ApiException
    {
        public WrongWorkoutPageFormat(string details)
            : base(400, "Format of page parameter is wrong", details)
        { }
    }
}
