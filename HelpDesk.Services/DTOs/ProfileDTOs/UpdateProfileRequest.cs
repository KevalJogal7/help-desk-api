namespace HelpDesk.Services.DTOs.ProfileDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class UpdateProfileRequest
{
    [Required(ErrorMessage = Messages.Validation.NameRequired)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = Messages.Validation.NameLength)]
    public string Name { get; set; } = null!;
}
