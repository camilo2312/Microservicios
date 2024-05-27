using ApiGateway.Core.Interfaces.Core;
using ApiGateway.Domain;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ApiGateway.Core.Services
{
    public class RabbitMqLogListener
    {
        private readonly IConfiguration _configuration;
        private readonly IUserProfileCore _userProfileCore;
        public RabbitMqLogListener(IUserProfileCore userProfileCore, IConfiguration configuration)
        {
            _userProfileCore = userProfileCore;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => StartListening());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void StartListening()
        {
            while (true)
            {
                try
                {
                    var factory = new ConnectionFactory() { Uri = new Uri(_configuration.GetSection("ConnectionStrings:RabbitMqConnection").Value) };

                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "registros",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);

                            // Registrar el perfil en la base de datos
                            if (!string.IsNullOrEmpty(message))
                            {
                                UserProfile user = JsonConvert.DeserializeObject<UserProfile>(message);
                                _userProfileCore.SaveProfile(user);
                            }

                            Console.WriteLine("Mensaje recibido: {0}", message);
                        };

                        channel.BasicConsume(queue: "registros",
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine("Escuchando mensajes de RabbitMQ");
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
