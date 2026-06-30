using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.Exceptions.InternalServerError
{
    public class NoInfoInJWTTokenExeption
        : ApiException
    {
        public NoInfoInJWTTokenExeption(string details)
            : base(500, "JWT token is provided but does not contain some information", details)
        { }
    }
}
