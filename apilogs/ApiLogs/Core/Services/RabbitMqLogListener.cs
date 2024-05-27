using ApiLogs.Core.Interfaces.Core;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using ApiLogs.Model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ApiLogs.Core.Services
{
    public class RabbitMqLogListener
    {
        private readonly IConfiguration _configuration;
        private readonly ILogsCore _logsCore;
        public RabbitMqLogListener(ILogsCore logsCore, IConfiguration configuration)
        {
            _logsCore = logsCore;
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
                        channel.QueueDeclare(queue: "autenticaciones",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);

                            // Registrar el mensaje en la base de datos de logs
                            if (!string.IsNullOrEmpty(message))
                            {
                                LogsDTO logs = JsonConvert.DeserializeObject<LogsDTO>(message);
                                _logsCore.RegisterLog(logs);
                            }

                            Console.WriteLine("Mensaje recibido: {0}", message);
                        };

                        channel.BasicConsume(queue: "autenticaciones",
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine("Escuchando mensajes de RabbitMQ. Presiona Enter para salir.");
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
