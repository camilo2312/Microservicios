using ApiHealth.Domain;

namespace ApiHealth.Core.Interfaces.Persistence
{
    public interface IHealthDataBase
    {
        ResponseServices GetHealthServiceByName(string serviceName);
        List<ResponseServices> GetHealthServices(bool isCheckService);
        bool SaveService(RequestService requestService);
    }
}
