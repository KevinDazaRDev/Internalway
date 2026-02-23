using System;

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
        string FirstName,
        string LastName,
        string Email,
        string? Phone,
        string? DocumentType,
        string? DocumentNumber
    );

    public record UpdateClientRequest(
        string FirstName,
        string LastName,
        string Email,
        string? Phone,
        string? DocumentType,
        string? DocumentNumber
    );
}
