using ApiHealth.Domain;

namespace ApiHealth.Core.Interfaces.Core
{
    public interface IHealthCore
    {
        bool SaveService(RequestService requestService);
        List<ResponseServices> GetHealthServices(bool isCheckServices);
        ResponseServices GetHealthServiceByName(string serviceName);
    }
}
