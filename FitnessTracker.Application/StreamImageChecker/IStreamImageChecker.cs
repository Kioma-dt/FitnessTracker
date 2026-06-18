using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.StreamImageChecker
{
    public interface IStreamImageChecker
    {
        Task<bool> IsSteamImage(Stream stream);
    }
}
