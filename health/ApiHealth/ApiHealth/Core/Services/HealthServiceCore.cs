using ApiHealth.Core.Interfaces.Core;
using ApiHealth.Core.Interfaces.Persistence;
using ApiHealth.Domain;

namespace ApiHealth.Core.Services
{
    public class HealthServiceCore : IHealthCore
    {
        private readonly IHealthDataBase _healthDataBase;

        public HealthServiceCore(IHealthDataBase healthDataBase)
        {
            _healthDataBase = healthDataBase;
        }
        public ResponseServices GetHealthServiceByName(string serviceName)
        {
            try
            {
                return _healthDataBase.GetHealthServiceByName(serviceName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ResponseServices> GetHealthServices(bool isCheckService)
        {
            try
            {
                return _healthDataBase.GetHealthServices(isCheckService);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveService(RequestService requestService)
        {
            try
            {
                return _healthDataBase.SaveService(requestService);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
