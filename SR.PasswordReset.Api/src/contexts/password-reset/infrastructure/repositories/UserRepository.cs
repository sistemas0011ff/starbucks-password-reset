

using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.entities;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;
using SR.PasswordReset.Api.src.contexts.shared.configuration;

namespace SR.PasswordReset.Api.src.contexts.password_reset.infrastructure.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _usersTableName;

        public UserRepository(
            IAmazonDynamoDB dynamoDbClient,
            AWSConfiguration awsConfig)  
        {
            _dynamoDbClient = dynamoDbClient;
            _usersTableName = awsConfig.UsersTableName;
        }

        public async Task<bool> ExistsById(Email email)
        {
            try
            {
                var getUserItem = new GetItemRequest
                {
                    TableName = _usersTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "Email", new AttributeValue { S = email.Value }}
                    }
                };

                var response = await _dynamoDbClient.GetItemAsync(getUserItem);
                return response.Item != null && response.Item.Any();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking user existence: {ex.Message}", ex);
            }
        }

        public async Task<User> GetById(Email email)
        {
            try
            {
                var getUserItem = new GetItemRequest
                {
                    TableName = _usersTableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "Email", new AttributeValue { S = email.Value }}
                    }
                };

                var response = await _dynamoDbClient.GetItemAsync(getUserItem);

                if (response.Item == null || !response.Item.Any())
                    return null;

                return User.Create(email.Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user: {ex.Message}", ex);
            }
        }
    }
}
