using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public string? ProfileImageUrl { get; private set; }

    public string? Bio { get; private set; }

    public bool IsActive { get; private set; }

    public string Role { get; private set; } = "User";

    public ArtistProfile? ArtistProfile { get; private set; }

    private User()
    {
    }

    private User(
        string username,
        string email,
        string passwordHash,
        string displayName)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        IsActive = true;
        Role = "User";
    }

    public static User Create(
        string username,
        string email,
        string passwordHash,
        string displayName)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(
                "Username is required.",
                nameof(username));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(email));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException(
                "Password hash is required.",
                nameof(passwordHash));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException(
                "Display name is required.",
                nameof(displayName));
        }

        return new User(
            username,
            email,
            passwordHash,
            displayName);
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException(
                "Password hash is required.",
                nameof(passwordHash));
        }

        PasswordHash = passwordHash;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}