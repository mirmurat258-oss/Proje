using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace API.Conf;
public static class ServiceCollection
{
    public static IServiceCollection AddScalarWithJWT(this IServiceCollection services)
    {
        services.AddOpenApi("v1", options =>
        {
            options.CustomSchemaIds(x => x.Name?.Replace("+", ".", StringComparison.Ordinal));
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<IJwtAuthService, JwtAuthManager>();

        //services.Configure<AuthenticationInfoOptions>(configuration.GetSection(nameof(AuthenticationInfoOptions)));
        //var authenticationInfoOptions = configuration.GetSection(nameof(AuthenticationInfoOptions)).Get<AuthenticationInfoOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }
        //   ) .AddJwtBearer(options =>
        //{
        //    var key = authenticationInfoOptions!.Key;
        //    var issuer = authenticationInfoOptions.Issuer;
        //    var audience = authenticationInfoOptions.Audience;

        //    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        //    options.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidateIssuer = true,
        //        ValidIssuer = issuer,
        //        ValidateAudience = true,
        //        ValidAudience = audience,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = symmetricKey
        //    };
        //    options.Events = new JwtBearerEvents
        //    {
        //        OnMessageReceived = context =>
        //        {
        //            var accessToken = context.Request.Query["access_token"];
        //            var path = context.HttpContext.Request.Path;
        //            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
        //            {
        //                context.Token = accessToken;
        //            }
        //            return Task.CompletedTask;
        //        }
        //    };
        //}
        );
        return services;
    }

    public static WebApplication UseScalarUI(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(delegate (ScalarOptions options)
            {
                options.Title = "Scalar UI";
                options.DarkMode = true;
                options.Favicon = "path";
                options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);
                options.HideModels = false;
                options.Layout = ScalarLayout.Modern;
                options.ShowSidebar = true;
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = "Bearer"
                };
            });
        }
        return app;
    }
}