using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions
{
    public class EnviormnetVariableNotFoundException 
        : ApiException
    {

            public EnviormnetVariableNotFoundException(string details)
                : base(500, "Enviorment variable not set on server", details)
            { }
    }
}
