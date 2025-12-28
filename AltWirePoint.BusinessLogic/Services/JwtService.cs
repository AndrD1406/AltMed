using AltWirePoint.BusinessLogic.Models.Identity;
using AltWirePoint.BusinessLogic.Services.Interfaces;
using AltWirePoint.DataAccess.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Services;

public class JwtService : IJwtService
{
    //to read Jwt configuration from appsettings.json
    private readonly IConfiguration configuration;
    // size of key must be greater than 256 bites
    private readonly string key;

    public JwtService(IConfiguration config)
    {
        configuration = config;
        key = configuration["Jwt:Key"];
    }

    public AuthenticationResponse CreateJwtToken(ApplicationUser user)
    {
        var accessExpiration = DateTime.UtcNow
            .AddMinutes(Convert.ToDouble(configuration["Jwt:AccessTokenExpirationMinutes"]));
        var refreshExpiration = DateTime.UtcNow
            .AddMinutes(Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationMinutes"]));

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Email),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

        var keyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: accessExpiration,
            signingCredentials: signingCredentials
        );
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new AuthenticationResponse
        {
            Token = encodedToken,
            Email = user.Email,
            UserName = user.UserName,
            Expiration = accessExpiration,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpirationDateTime = refreshExpiration
        };
    }

    //Creates a refresh token (base 64 string of random numbers)
    private string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),

            //method called when token is expired
            ValidateLifetime = false,
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = null;

        try
        {
            principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
            throw;
        }

        return principal;
    }
}
