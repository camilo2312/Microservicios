using ApiGateway.Domain;

namespace ApiGateway.Core.Interfaces.Persistence
{
    public interface IUserProfileDataBase
    {
        bool SaveProfile(UserProfile userProfile);
        UserProfile GetUserProfile(string userId);
        bool UpdateUserProfile(string userId, UserProfile userProfile);
    }
}
