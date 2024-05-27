using ApiGateway.Core.Interfaces.Core;
using ApiGateway.Core.Interfaces.Persistence;
using ApiGateway.Domain;

namespace ApiGateway.Core.Services
{
    public class UserProfileServiceCore : IUserProfileCore
    {
        private readonly IUserProfileDataBase _userProfileDataBase;

        public UserProfileServiceCore(IUserProfileDataBase userProfileDataBase)
        {
            _userProfileDataBase = userProfileDataBase;
        }
        public UserProfile GetUserProfile(string userId)
        {
            try
            {
                return _userProfileDataBase.GetUserProfile(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveProfile(UserProfile profile)
        {
            try
            {
                return _userProfileDataBase.SaveProfile(profile);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateUserProfile(string userId, UserProfile userProfile)
        {
            try
            {
                return _userProfileDataBase.UpdateUserProfile(userId, userProfile);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
