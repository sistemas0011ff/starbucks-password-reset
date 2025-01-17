

namespace SR.PasswordReset.Api.src.contexts.shared.configuration
{
    public class AWSConfiguration
    {
        public string UserPoolId { get; init; }
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
        public string UsersTableName { get; init; }

        public AWSConfiguration()
        {
            UserPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID");
            ClientId = Environment.GetEnvironmentVariable("COGNITO_CLIENT_ID");
            ClientSecret = Environment.GetEnvironmentVariable("COGNITO_CLIENT_SECRET");
            UsersTableName = Environment.GetEnvironmentVariable("USERS_TABLE_NAME") ?? "SBUsers";
        }
    }
}
