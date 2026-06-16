using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
