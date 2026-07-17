namespace HelpDesk.Services.DTOs.ProfileDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = Messages.Validation.PasswordRequired)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = Messages.Validation.PasswordRequired)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = Messages.Validation.PasswordLength)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&^#()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
        ErrorMessage = Messages.Validation.PasswordComplexity
    )]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = Messages.Validation.PasswordRequired)]
    [Compare(nameof(NewPassword), ErrorMessage = Messages.Profile.PasswordMismatch)]
    public string ConfirmPassword { get; set; } = null!;
}
