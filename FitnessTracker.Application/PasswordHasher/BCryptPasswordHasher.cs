using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.PasswordHasher
{
    public class BCryptPasswordHasher
        : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
