using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.ValidationAttributes
{
    public class NoWhiteSpacesAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is String str)
            {
                return !str.Any(char.IsWhiteSpace);
            }

            return true;
        }
    }
}
