using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.Interfaces
{
    public interface IStreamImageChecker
    {
        Task<bool> IsSteamImage(Stream stream);
    }
}
