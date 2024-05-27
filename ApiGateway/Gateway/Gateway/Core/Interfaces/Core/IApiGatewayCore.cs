using Gateway.Domain;

namespace Gateway.Core.Interfaces.Core
{
    public interface IApiGatewayCore
    {
        Task<HttpResponseMessage> AuthenticateUserAsync(string email, string password);
        Task<HttpResponseMessage> RegisterUserAsync(User user);
        Task<ResponseInfoUser> GetUserProfileAsync(string userId);
        Task<HttpResponseMessage> UpdateUserProfileAsync(string userId, UserProfile userProfile);
    }
}
