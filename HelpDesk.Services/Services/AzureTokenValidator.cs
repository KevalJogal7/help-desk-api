namespace HelpDesk.Services.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AzureTokenValidator
{
    private readonly IConfiguration _configuration;

    public AzureTokenValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ClaimsPrincipal> ValidateAsync(string token)
    {
        var tenantId = _configuration["AzureAd:TenantId"];
        var clientId = _configuration["AzureAd:ClientId"];

        var authority =
            $"https://login.microsoftonline.com/{tenantId}/v2.0";

        var configurationManager =
            new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

        var openIdConfig =
            await configurationManager.GetConfigurationAsync();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{authority}",

            ValidateAudience = true,
            ValidAudience = clientId,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = openIdConfig.SigningKeys
        };

        var handler = new JwtSecurityTokenHandler();

        var principal = handler.ValidateToken(
            token,
            validationParameters,
            out SecurityToken validatedToken);

        return principal;
    }
}