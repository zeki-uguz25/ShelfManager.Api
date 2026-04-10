using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Services;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
}
