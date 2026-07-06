namespace HelpDesk.Services.DTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class RegisterRequest
{
    [Required(ErrorMessage = Messages.Validation.FirstNameRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Messages.Validation.FirstNameLength)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = Messages.Validation.LastNameRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Messages.Validation.LastNameLength)]
    public string LastName { get; set; }

    [Required(ErrorMessage = Messages.Validation.EmailRequired)]
    [EmailAddress(ErrorMessage = Messages.Validation.EmailInvalid)]
    [StringLength(255, ErrorMessage = Messages.Validation.EmailLength)]
    public string Email { get; set; }

    [Required(ErrorMessage = Messages.Validation.PasswordRequired)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = Messages.Validation.PasswordLength)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&^#()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
        ErrorMessage = Messages.Validation.PasswordComplexity
    )]
    public string Password { get; set; }
}
