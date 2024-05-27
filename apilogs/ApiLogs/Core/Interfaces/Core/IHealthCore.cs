using ApiLogs.Model;

namespace ApiLogs.Core.Interfaces.Core
{
    public interface IHealthCore
    {
        FullHealth GetHealth();
        Health GetReady();
        Health GetLive();
    }
}
