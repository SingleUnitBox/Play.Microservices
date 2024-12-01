namespace Play.Common.Auth;

public class AuthSettings
{
    public string IssuerSigningKey { get; set; }
    public string Issuer { get; set; }
    public string ValidIssuer { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool     ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public TimeSpan Expiry { get; set; }
}