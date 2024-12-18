using System.ComponentModel.DataAnnotations;

public class TagValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var tag = value as string;
        return tag.ToLower() is "home" or "business" or "personal" 
            ? ValidationResult.Success
            : new ValidationResult("The Tag must be either 'Home' or 'Business' 'Personal'.");
    }
}