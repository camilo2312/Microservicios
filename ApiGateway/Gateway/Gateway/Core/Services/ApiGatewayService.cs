using Gateway.Core.Interfaces.Core;
using Gateway.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Gateway.Core.Services
{
    public class ApiGatewayService : IApiGatewayCore
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiGatewayService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> AuthenticateUserAsync(string email, string password)
        {
            string apiUrl = _configuration.GetSection("ConnectionUsersApiUrl").Value + "/login";
            string requestBody = JsonConvert.SerializeObject(new { email, password });

            Logs logs = new Logs()
            {
                Application = "Api Gateway",
                LogType = "INFORMATION",
                Module = "Autenticación",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de autenticar al usuario"
            };

            await SaveLog(logs);

            return await _httpClient.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> RegisterUserAsync(User user)
        {
            string apiUrl = _configuration.GetSection("ConnectionUsersApiUrl").Value + "/users";
            string requestBody = JsonConvert.SerializeObject(user);

            Logs logs = new Logs()
            {
                Application = "Api Gateway",
                LogType = "INFORMATION",
                Module = "Registro",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de registrar al usuario"
            };

            await SaveLog(logs);

            return await _httpClient.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
        }


        public async Task<ResponseInfoUser> GetUserProfileAsync(string userId)
        {
            ResponseInfoUser responseInfoUser = new ResponseInfoUser();
            string apiUrl = _configuration.GetSection("ConnectionProfileUsersURL").Value + $"/{userId}";
            string apiUrlUsers = _configuration.GetSection("ConnectionUsersApiUrl").Value + $"/user/{userId}";

            var responseProfile = await _httpClient.GetAsync(apiUrl);
            var responseUser = await _httpClient.GetAsync(apiUrlUsers);

            responseInfoUser.Profile = JsonConvert.DeserializeObject<UserProfile>(await responseProfile.Content.ReadAsStringAsync());
            var obj = JsonConvert.DeserializeObject<JObject>(await responseUser.Content.ReadAsStringAsync());

            responseInfoUser.Information = JsonConvert.DeserializeObject<User>(obj["data"][0].ToString());

            Logs logs = new Logs()
            {
                Application = "Api Gateway",
                LogType = "INFORMATION",
                Module = "Obtener perfil de usuario",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de obtener el perfil del usuario"
            };

            await SaveLog(logs);

            return responseInfoUser;
        }

        public async Task<HttpResponseMessage> UpdateUserProfileAsync(string userId, UserProfile userProfile)
        {
            string apiUrl = _configuration.GetSection("ConnectionProfileUsersURL").Value + $"/{userId}";
            string requestBody = JsonConvert.SerializeObject(userProfile);

            Logs logs = new Logs()
            {
                Application = "Api Gateway",
                LogType = "INFORMATION",
                Module = "Actualizar perfil de usuario",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de actualizar el perfil del usuario"
            };

            await SaveLog(logs);

            return await _httpClient.PutAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
        }

        private async Task SaveLog(Logs logs)
        {
            string urlServiceLogs = _configuration.GetSection("ConnectionLogsUrl").Value;
            if (logs != null)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(logs), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(urlServiceLogs, content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error al registrar el log");
                }
                else
                {
                    Console.WriteLine("Log registrado correctamente");
                }
            }
        }
    }
}
