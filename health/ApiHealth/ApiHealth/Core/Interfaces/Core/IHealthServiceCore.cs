using ApiHealth.Domain;

namespace ApiHealth.Core.Interfaces.Core
{
    public interface IHealthServiceCore
    {
        FullHealth GetHealth();
        Health GetReady();
        Health GetLive();
    }
}
