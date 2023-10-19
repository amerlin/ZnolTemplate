
using Microsoft.AspNetCore.Identity;

namespace ZnolBe.Shared.Models.Auth;
public class ZnolBeApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; } = null!;
    public DateTime? RefreshTokenExpiryTime { get; set; }
}