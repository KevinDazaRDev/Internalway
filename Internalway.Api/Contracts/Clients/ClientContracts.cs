using System;
using System.ComponentModel.DataAnnotations;

namespace Internalway.Api.Contracts.Clients
{
    public record ClientDto(
        long Id,
        string FirstName,
        string LastName,
        string Email,
        string? Phone,
        string? DocumentType,
        string? DocumentNumber,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    public record CreateClientRequest(
        [Required, MaxLength(200)] string FirstName,
        [Required, MaxLength(200)] string LastName,
        [Required, EmailAddress, MaxLength(320)] string Email,
        [MaxLength(50)] string? Phone,
        [MaxLength(50)] string? DocumentType,
        [MaxLength(100)] string? DocumentNumber
    );

    public record UpdateClientRequest(
        [Required, MaxLength(200)] string FirstName,
        [Required, MaxLength(200)] string LastName,
        [Required, EmailAddress, MaxLength(320)] string Email,
        [MaxLength(50)] string? Phone,
        [MaxLength(50)] string? DocumentType,
        [MaxLength(100)] string? DocumentNumber
    );
}
