namespace ShelfManager.Application.Abstractions.Services;

public interface IHashingService
{
    string HashPassword(string password, out string salt);
    bool VerifyPassword(string password, string hash, string salt);
}
