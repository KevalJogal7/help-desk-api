namespace HelpDesk.Services.DTOs.ForgotPasswordDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = Messages.Validation.EmailRequired)]
    [EmailAddress(ErrorMessage = Messages.Validation.EmailInvalid)]
    [StringLength(255, ErrorMessage = Messages.Validation.EmailLength)]
    public string Email { get; set; }

}
