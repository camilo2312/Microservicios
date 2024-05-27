using ApiHealth.Core.Interfaces.Persistence;
using ApiHealth.Domain;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiHealth.Persistence
{
    public class HealthDataBase : IHealthDataBase
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection connection = null;

        public HealthDataBase(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new MySqlConnection(_configuration.GetSection("ConnectionStrings:ConnectionMysql").Value);
        }

        public ResponseServices GetHealthServiceByName(string serviceName)
        {
            ResponseServices response = null;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine($"SELECT Id, Name, EndPoint FROM healthservice WHERE Name LIKE '%{serviceName}%'");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection)) 
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        response = new ResponseServices();
                        response.Id = reader.GetInt32(0);
                        response.Name = reader.GetString(1);
                        response.EndPoint = reader.GetString(2);
                    }
                }
            }

            if (response != null) 
            {
                response.Health = GetHealthServiceEndPoint(response.EndPoint);
            }

            return response;
        }

        public List<ResponseServices> GetHealthServices(bool isCheckService)
        {
            List<ResponseServices> lstResponse = new List<ResponseServices>();
            ResponseServices response = null;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine($"SELECT Id, Name, EndPoint, Emails FROM healthservice");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response = new ResponseServices();
                        response.Id = reader.GetInt32(0);
                        response.Name = reader.GetString(1);
                        response.EndPoint = reader.GetString(2);
                        response.Emails = reader.GetString(3);

                        lstResponse.Add(response);
                    }
                }
            }

            if (lstResponse.Any() && !isCheckService)
            {
                foreach (ResponseServices responseServices in lstResponse)
                {
                    response.Health = GetHealthServiceEndPoint(responseServices.EndPoint);
                }
            }

            return lstResponse;
        }

        public bool SaveService(RequestService requestService)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine("INSERT INTO healthservice (Name, Frequency, EndPoint, Emails) ");
            queryString.AppendLine($"VALUES ('{requestService.Name}', {requestService.Frequency}, ");
            queryString.AppendLine($"'{requestService.EndPoint}', '{requestService.Emails}')");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection))
            {
                response = command.ExecuteNonQuery();
            }

            return response > 0;
        }

        private object GetHealthServiceEndPoint(string endPoint)
        {
            object value = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(endPoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    value = JsonConvert.DeserializeObject<object>(jsonResponse);
                }
            }

            return value;
        }
    }
}
