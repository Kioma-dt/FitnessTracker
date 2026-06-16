using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class EnviormnetVariableNotFoundException 
        : Exception
    {
        public EnviormnetVariableNotFoundException() 
            :base()
        { }

        public EnviormnetVariableNotFoundException(string message)
            : base(message)
        { }
    }
}
