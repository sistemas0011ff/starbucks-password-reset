using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SR.PasswordReset.Api;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using SR.PasswordReset.Api.src.app.handlers.password_reset.dtos;
using System.Text.Json.Serialization.Metadata;
using SR.PasswordReset.Api.src.contexts.shared.responses;
using System.ComponentModel.DataAnnotations;

[assembly: LambdaGlobalProperties(GenerateMain = true)]
[assembly: LambdaSerializer(typeof(SourceGeneratorLambdaJsonSerializer<LambdaFunctionJsonSerializerContext>))]

namespace SR.PasswordReset.Api;

[JsonSerializable(typeof(APIGatewayProxyRequest), TypeInfoPropertyName = "ProxyRequest")]
[JsonSerializable(typeof(APIGatewayProxyResponse), TypeInfoPropertyName = "ProxyResponse")]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest), TypeInfoPropertyName = "HttpApiRequest")]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse), TypeInfoPropertyName = "HttpApiResponse")]

// Tipos de contexto y certificados
// Tipos de contexto y certificados
[JsonSerializable(typeof(APIGatewayProxyRequest), TypeInfoPropertyName = "ProxyRequest")]
[JsonSerializable(typeof(APIGatewayProxyResponse), TypeInfoPropertyName = "ProxyResponse")]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest), TypeInfoPropertyName = "HttpApiRequest")]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse), TypeInfoPropertyName = "HttpApiResponse")]


// DTOs de Request
[JsonSerializable(typeof(RequestOtpRequestDto))]
[JsonSerializable(typeof(ValidateOtpRequestDto))]
[JsonSerializable(typeof(SendOtpRequestDto))]
[JsonSerializable(typeof(ResetPasswordRequestDto))]

// DTOs de Response y Base
[JsonSerializable(typeof(SR.PasswordReset.Api.src.contexts.shared.responses.BaseResponse), TypeInfoPropertyName = "BaseResponseType")]
[JsonSerializable(typeof(ErrorResponseDto))]
[JsonSerializable(typeof(RequestOtpResponseDto))]
[JsonSerializable(typeof(ValidateOtpResponseDto))]
[JsonSerializable(typeof(SendOtpResponseDto))]
[JsonSerializable(typeof(ResetPasswordResponseDto))]

// AWS Configuration
[JsonSerializable(typeof(SR.PasswordReset.Api.src.contexts.shared.configuration.AWSConfiguration), TypeInfoPropertyName = "AWSConfig")]

public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext
{
}

public class Functions
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly string _userPoolId;
    private readonly string _clientId;
    private readonly string _usersTableName;

    public Functions()
    {
        _cognitoClient = new AmazonCognitoIdentityProviderClient();
        _dynamoDbClient = new AmazonDynamoDBClient();
        _userPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID") ?? throw new InvalidOperationException("COGNITO_USER_POOL_ID no configurado");
        _clientId = Environment.GetEnvironmentVariable("COGNITO_CLIENT_ID") ?? throw new InvalidOperationException("COGNITO_CLIENT_ID no configurado");
        _usersTableName = Environment.GetEnvironmentVariable("USERS_TABLE_NAME") ?? throw new InvalidOperationException("USERS_TABLE_NAME no configurado");
    }
}
