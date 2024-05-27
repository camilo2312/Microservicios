using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ApiLogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly ConnectionFactory _factory;

        public RabbitController()
        {
            _factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        }

        [HttpGet]
        public IActionResult Consume() 
        {
            try
            {
                using (var connection = _factory.CreateConnection())
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
                        Console.WriteLine("Mensaje recibido: {0}", message);
                    };
                    channel.BasicConsume(queue: "autenticaciones",
                                         autoAck: true,
                                         consumer: consumer);

                    return Ok("Consumidor iniciado. Presiona Enter para salir.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al iniciar el consumidor: {ex.Message}");
            }
        }
    }
}
