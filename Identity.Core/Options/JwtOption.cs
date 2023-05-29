namespace Identity.Core.Options;

public class JwtOption
{
    public required string SignInKey { get; set; }
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public int ExpiresInMinute { get; set; }
}
