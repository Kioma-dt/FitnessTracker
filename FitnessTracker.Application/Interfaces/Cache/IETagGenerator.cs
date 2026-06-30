using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.Interfaces.Cache
{
    public interface IETagGenerator
    {
        string Generate(object value);
    }
}
