namespace Account.Application;

public record JwtInformation(
    string SecretKey,
    string Issuer,
    string Audience);