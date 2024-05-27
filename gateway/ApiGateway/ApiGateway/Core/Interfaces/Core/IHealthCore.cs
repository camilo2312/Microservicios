using ApiGateway.Domain;

namespace ApiGateway.Core.Interfaces.Core
{
    public interface IHealthCore
    {
        FullHealth GetHealth();
        Health GetReady();
        Health GetLive();
    }
}
