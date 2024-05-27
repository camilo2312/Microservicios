using ApiGateway.Core.Interfaces.Persistence;
using ApiGateway.Domain;
using ApiGateway.Domain.DTO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ApiGateway.Persistence
{
    public class UserProfileDataBase : IUserProfileDataBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private MySqlConnection connection = null;

        public UserProfileDataBase(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new MySqlConnection(_configuration.GetSection("ConnectionStrings:ConnectionMysql").Value);
            _httpClient = new HttpClient();
        }
        public UserProfile GetUserProfile(string userId)
        {
            UserProfile userProfile = null;
            StringBuilder queryString = new StringBuilder();
            queryString.AppendLine($"SELECT * FROM profiles.user_profile WHERE Id = {userId}");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    string json = JsonConvert.SerializeObject(table);

                    if (!json.Equals("[]"))
                    {
                        UserProfileDTO userProfileDTO = JsonConvert.DeserializeObject<List<UserProfileDTO>>(json)[0];

                        userProfile = new UserProfile()
                        {
                            Biography = userProfileDTO.Biography,
                            Country = userProfileDTO.Country,
                            IsContactInfoPublic = userProfileDTO.IsContactInfoPublic,
                            MailingAddress = userProfileDTO.MailingAddress,
                            Nickname = userProfileDTO.Nickname,
                            Organization = userProfileDTO.Organization,
                            PersonalPageUrl = userProfileDTO.PersonalPageUrl,
                            SocialLinks = userProfileDTO.SocialLinks.Split(",").ToList(),
                            UserId = userProfileDTO.UserId
                        };
                    }
                }
            }

            Logs logs = new Logs()
            { 
                Application = "Gateway",
                LogType = "INFORMATION",
                Module = "Obtener perfil de usuario",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de obtener el perfil de un usuario"
            };

            SaveLog(logs);

            return userProfile;
        }

        public bool SaveProfile(UserProfile userProfile)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine("INSERT INTO user_profile ");
            queryString.AppendLine("(PersonalPageUrl, NickName, IsContactInfoPublic, ");
            queryString.AppendLine("MailingAddress, Biography, Organization, Country, SocialLinks, Id)");
            queryString.AppendLine("VALUES ( ");
            queryString.AppendLine($"'{userProfile.PersonalPageUrl}', ");
            queryString.AppendLine($"'{userProfile.Nickname}', '{userProfile.IsContactInfoPublic}', ");
            queryString.AppendLine($"'{userProfile.MailingAddress}', '{userProfile.Biography}', ");
            queryString.AppendLine($"'{userProfile.Organization}', '{userProfile.Country}', ");
            queryString.AppendLine($"'{string.Join(",", userProfile.SocialLinks)}',");
            queryString.AppendLine($"'{userProfile.Id}')");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection))
            {
                response = command.ExecuteNonQuery();
            }

            Logs logs = new Logs()
            {
                Application = "Gateway",
                LogType = "INFORMATION",
                Module = "Almacenando perfil",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de almacenar el perfil del usuario"
            };

            SaveLog(logs);

            return response > 0;
        }

        public bool UpdateUserProfile(string userId, UserProfile userProfile)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine("UPDATE user_profile SET ");
            queryString.AppendLine($"PersonalPageUrl = '{userProfile.PersonalPageUrl}', ");
            queryString.AppendLine($"Nickname = '{userProfile.Nickname}', ");
            queryString.AppendLine($"IsContactInfoPublic = '{userProfile.IsContactInfoPublic}', ");
            queryString.AppendLine($"MailingAddress = '{userProfile.MailingAddress}', ");
            queryString.AppendLine($"Biography = '{userProfile.Biography}', ");
            queryString.AppendLine($"Organization = '{userProfile.Organization}', ");
            queryString.AppendLine($"Country = '{userProfile.Country}', ");
            queryString.AppendLine($"SocialLinks = '{string.Join(",", userProfile.SocialLinks)}'");
            queryString.AppendLine($"WHERE Id = {userProfile.Id}");

            if (!(connection.State == ConnectionState.Open))
            {
                connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), connection))
            {
                response = command.ExecuteNonQuery();
            }

            Logs logs = new Logs()
            {
                Application = "Gateway",
                LogType = "INFORMATION",
                Module = "Actualización de perfil",
                Timestamp = DateTime.Now,
                Summary = "",
                Description = "End point encargado de actualizar el perfil del usuario"
            };

            SaveLog(logs);


            return response > 0;
        }

        private void SaveLog(Logs logs)
        {
            string urlServiceLogs = _configuration.GetSection("ConnectionLogsUrl").Value;
            if (logs != null)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(logs), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(urlServiceLogs, content).Result;
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
