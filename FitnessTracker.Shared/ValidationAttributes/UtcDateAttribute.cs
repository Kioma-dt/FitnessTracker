using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.ValidationAttributes
{
    public class UtcDateAttribute :
        ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date 
                && date.Kind != DateTimeKind.Utc)
            {
                return false;
            }

            return true;
        }
    }
}
