using Identity.Core.Context;
using Identity.Core.Managers;
using Identity.Core.Options;
using Identity.Core.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Core.Extentions;

public static class ServiceCollectionExtentions
{
    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(JwtOption));
        services.Configure<JwtOption>(section);
        var jwtOptions = section.Get<JwtOption>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var signInKey = System.Text.Encoding.UTF32.GetBytes(jwtOptions.SignInKey);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signInKey),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwt(configuration);

        services.AddScoped<JwtTokenManager>();
        services.AddScoped<UserManager>();

        services.AddHttpContextAccessor();
        services.AddScoped<UserProvider>();
    }

    public static void MigrateIdentityDb(this WebApplication app)
    {
        if (app.Services.GetService<IdentityDbContext>() != null)
        {
            var identityDb = app.Services.GetRequiredService<IdentityDbContext>();
            identityDb.Database.Migrate();
        }
    }

}
