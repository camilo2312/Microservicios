using Gateway.Core.Interfaces.Core;
using Gateway.Domain;

namespace Gateway.Core.Services
{
    public class HealthServiceCore : IHealthCore
    {
        private readonly DateTime startTime = DateTime.UtcNow;
        public FullHealth GetHealth()
        {
            try
            {
                FullHealth fullHealth = new FullHealth();
                fullHealth.status = "UP";
                fullHealth.checks.Add(GetReady());
                fullHealth.checks.Add(GetLive());

                return fullHealth;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Health GetLive()
        {
            try
            {
                TimeSpan uptime = DateTime.UtcNow - startTime;
                return new Health
                {
                    status = "LIVE",
                    uptime = uptime
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Health GetReady()
        {
            try
            {
                TimeSpan uptime = DateTime.UtcNow - startTime;
                return new Health
                {
                    status = "READY",
                    uptime = uptime
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
