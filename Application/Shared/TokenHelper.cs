using Domain.Entities.UserEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Shared
{
    public static class TokenHelper
    {
        public static string GenerateToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new Dictionary<string, string>()
        {
            { "email_address", user.Email },
        };

            var accessToken = GenerateAccessToken(user.Id, claims);
            return accessToken;
        }

        private static string GenerateAccessToken(int id, IDictionary<string, string> claims)
        {
            var convertedClaims = claims?.Select(x => new Claim(x.Key, x.Value)).ToList() ?? new List<Claim>();
            convertedClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, id.ToString()));
            convertedClaims.Add(new Claim(ClaimTypes.Name, id.ToString()));

            var accessToken = GenerateJwt(convertedClaims);
            var tokenResponse = new JwtSecurityTokenHandler().WriteToken(accessToken);

            return tokenResponse;
        }

        private static JwtSecurityToken GenerateJwt(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B374A26A71490437AA024E4FADD5B497FDFF1A8EA6FF12F6FB65AF2720B59CCF"));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
