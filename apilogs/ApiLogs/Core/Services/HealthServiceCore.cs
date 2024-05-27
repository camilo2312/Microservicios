using ApiLogs.Core.Interfaces.Core;
using ApiLogs.Model;
using MySql.Data.MySqlClient;

namespace ApiLogs.Core.Services
{
    public class HealthServiceCore : IHealthCore
    {
        private readonly DateTime startTime = DateTime.UtcNow;
        private readonly IConfiguration _configuration;
        private MySqlConnection conection = null;

        public HealthServiceCore(IConfiguration configuration)
        {
            _configuration = configuration;
            conection = new MySqlConnection(_configuration.GetSection("ConnectionStrings:ConnectionMysql").Value);
        }

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
                    status = GetStatusReady(),
                    uptime = uptime
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string GetStatusReady()
        {
            try
            {
                conection.Open();
                if (conection.State == System.Data.ConnectionState.Open)
                {
                    conection.Close();
                    return "READY";
                }

                return "NO-READY";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "NO-READY";
                throw;
            }
        }
    }
}
