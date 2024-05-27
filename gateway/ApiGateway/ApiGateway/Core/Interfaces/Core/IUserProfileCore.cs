using ApiGateway.Domain;

namespace ApiGateway.Core.Interfaces.Core
{
    public interface IUserProfileCore
    {
        bool SaveProfile(UserProfile profile);
        UserProfile GetUserProfile(string userId);
        bool UpdateUserProfile(string userId, UserProfile userProfile);
    }
}
