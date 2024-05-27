using Gateway.Domain;

namespace Gateway.Core.Interfaces.Core
{
    public interface IHealthCore
    {
        FullHealth GetHealth();
        Health GetReady();
        Health GetLive();
    }
}
