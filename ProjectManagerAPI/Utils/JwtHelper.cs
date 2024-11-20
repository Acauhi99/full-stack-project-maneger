using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ProjectManagerAPI.Models;

public class JwtHelper
{
    private readonly string _secret;

    public JwtHelper(string secret)
    {
        _secret = secret;
    }

    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var header = new { alg = "HS256", typ = "JWT" };
        var payload = new
        {
            sub = user.Id,
            email = user.Email,
            role = user.TipoUsuario.ToString(),
            exp = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds()
        };

        string headerJson = JsonSerializer.Serialize(header);
        string payloadJson = JsonSerializer.Serialize(payload);

        string headerBase64 = Base64UrlEncode(headerJson);
        string payloadBase64 = Base64UrlEncode(payloadJson);

        string unsignedToken = $"{headerBase64}.{payloadBase64}";
        string signature = ComputeHmacSha256(unsignedToken, _secret);

        return $"{unsignedToken}.{signature}";
    }

    public bool ValidateToken(string token, out Dictionary<string, object>? payload)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token));

        payload = null;
        var parts = token.Split('.');
        if (parts.Length != 3) return false;

        string unsignedToken = $"{parts[0]}.{parts[1]}";
        string signature = ComputeHmacSha256(unsignedToken, _secret);

        if (signature != parts[2]) return false;

        var payloadJson = Base64UrlDecode(parts[1]);
        payload = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);

        if (payload != null && payload.ContainsKey("exp") &&
            DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(payload["exp"], CultureInfo.InvariantCulture)).UtcDateTime < DateTime.UtcNow)
            return false;

        return payload != null;
    }

    private static string ComputeHmacSha256(string data, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Base64UrlEncode(hash);
    }

    private static string Base64UrlEncode(string input) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string Base64UrlEncode(byte[] input) =>
        Convert.ToBase64String(input)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string Base64UrlDecode(string input) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(
            input.Replace('-', '+').Replace('_', '/').PadRight(input.Length + (4 - input.Length % 4) % 4, '=')));
}
