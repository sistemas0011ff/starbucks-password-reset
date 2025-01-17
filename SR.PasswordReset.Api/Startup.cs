using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Amazon.Lambda.Logging.AspNetCore;
using SR.PasswordReset.Api.src.app.handlers.password_reset;
using SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services;
using SR.PasswordReset.Api.src.contexts.password_reset.application.services;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.infrastructure.repositories;
using SR.PasswordReset.Api.src.contexts.shared.configuration;

namespace SR.PasswordReset.Api;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 1. Configurar Logging
        services.AddLogging(builder =>
        {
            builder.AddLambdaLogger();
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug); 
        });

        // 2. Clientes AWS
        services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient());
        services.AddSingleton<IAmazonCognitoIdentityProvider>(new AmazonCognitoIdentityProviderClient());

        // 3. Configuración
        services.AddSingleton(new AWSConfiguration());

        // 4. Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICognitoRepository, CognitoRepository>();

        // 5. Application Services
        services.AddScoped<IRequestOtpService, RequestOtpService>();
        services.AddScoped<IValidateOtpService, ValidateOtpService>();
        services.AddScoped<ISendOtpService, SendOtpService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();

        // 6. Handlers
        services.AddSingleton<RequestOtpHandler>();
        services.AddSingleton<ValidateOtpHandler>();
        services.AddSingleton<SendOtpHandler>();
        services.AddSingleton<ResetPasswordHandler>();

    }
}