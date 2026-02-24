using System.ComponentModel.DataAnnotations;

namespace Internalway.Api.Contracts.Auth
{
    public record RegisterRequest(
        [Required, EmailAddress, MaxLength(320)] string Email,
        [Required, MinLength(8), MaxLength(128)] string Password
    );

    public record LoginRequest(
        [Required, EmailAddress, MaxLength(320)] string Email,
        [Required, MinLength(8), MaxLength(128)] string Password
    );
}
