using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.ValidationAttributes
{
    public class NotFutureDateAttribute 
        : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date.ToUniversalTime() <= DateTime.UtcNow;
            }

            return true;
        }
    }
}
