using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class EnviormnetVariableNotFound 
        : Exception
    {
        public EnviormnetVariableNotFound() 
            :base()
        { }

        public EnviormnetVariableNotFound(string message)
            : base(message)
        { }
    }
}
