using ApiGateway.Core.Interfaces.Core;
using ApiGateway.Domain;
using MySql.Data.MySqlClient;
using System.Net.Http;

namespace ApiGateway.Core.Services
{
    public class HealthServiceCore : IHealthCore
    {
        private readonly DateTime startTime = DateTime.UtcNow;
        private MySqlConnection connection = null;
        private readonly IConfiguration _configuration;

        public HealthServiceCore(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new MySqlConnection(_configuration.GetSection("ConnectionStrings:ConnectionMysql").Value);
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
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    return "READY";
                }

                return "NO-READY";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "NO-READY";               
            }
        }
    }
}
