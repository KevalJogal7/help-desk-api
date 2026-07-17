namespace HelpDesk.Services.DTOs.UserDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class UpsertUserRequest
{
    // Null = create, has value = update
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = Messages.Validation.NameRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Messages.Validation.NameLength)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = Messages.Validation.EmailRequired)]
    [EmailAddress(ErrorMessage = Messages.Validation.EmailInvalid)]
    [StringLength(255, ErrorMessage = Messages.Validation.EmailLength)]
    public string Email { get; set; } = null!;

    // Only required on create
    [StringLength(100, MinimumLength = 8, ErrorMessage = Messages.Validation.PasswordLength)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&^#()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
        ErrorMessage = Messages.Validation.PasswordComplexity
    )]
    public string? Password { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Messages.Validation.RoleRequired)]
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
}
