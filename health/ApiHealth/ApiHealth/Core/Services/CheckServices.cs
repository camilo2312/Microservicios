using ApiHealth.Core.Interfaces.Core;
using ApiHealth.Domain;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace ApiHealth.Core.Services
{
    public class CheckServices : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        List<ResponseServices> lstServices = new List<ResponseServices>();
        private ResponseServices responseServicesEmail = new ResponseServices();

        public CheckServices(IServiceScopeFactory serviceScopeFactory)
        {
            _httpClient = new HttpClient();
            _serviceScopeFactory = serviceScopeFactory;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var healthCore = scope.ServiceProvider.GetRequiredService<IHealthCore>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    lstServices = healthCore.GetHealthServices(true);

                    foreach (ResponseServices responseServices in lstServices)
                    {
                        try
                        {
                                responseServicesEmail = responseServices;
                                HttpResponseMessage response = await _httpClient.GetAsync(responseServices.EndPoint, stoppingToken);
                                if (!response.IsSuccessStatusCode)
                                {
                                    SendEmail(configuration);
                                    Console.WriteLine($"El microservicio está en estado de alarma. Enviar notificación a los usuarios.");
                                
                                }
                                else
                                {
                                    Console.WriteLine($"El microservicio está en funcionamiento normal.");
                                }
                        }
                        catch (Exception ex)
                        {

                            SendEmail(configuration);
                            Console.WriteLine($"Error al verificar la salud del microservicio: {ex.Message}");
                        }
                    }
                }


                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Verificar cada minuto
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Método encargado de enviar el email
        /// por medio de la api de notificaciones
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private async Task SendEmail(IConfiguration configuration)
        {
            try
            {
                string endPoint = configuration.GetSection("ConnectNotificationApi").Value;

                EmailApi emailapi = new EmailApi()
                {
                    to = responseServicesEmail.Emails,
                    subject = "Notificación de servicio",
                    text = $"El servicio {responseServicesEmail.Name} no esta funcionando adecuadamente, por favor revisalo"
                };

                string requestBody = JsonConvert.SerializeObject(emailapi);

                HttpResponseMessage response = await _httpClient.PostAsync(endPoint, new StringContent(requestBody, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}
