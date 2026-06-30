using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.ValidationAttributes
{
    public class UrlListAttribute 
        : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not List<string> urls)
                return true;

            foreach (var url in urls)
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
