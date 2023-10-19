namespace ZnolBe.Shared.Configuration;

public class JwtOptionsConfig
{
    public const string JWTCONFIG = "Jwt:Config";

    public bool ValidateIssuer { get; set; }

    public bool ValidateAudience { get; set; }

    public bool ValidateLifetime { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    public string ValidIssuer { get; set; } = null!;

    public string ValidAudience { get; set; } = null!;

    public string Secret { get; set; } = null!;

    public string CookieName { get; set; } = null!;

    public int CookieDayExpire { get; set; }

    public int ExpirationMinutes { get; set; }
}
