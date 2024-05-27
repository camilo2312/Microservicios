using Moq;
using ApiHealth.Core.Interfaces.Persistence;
using ApiHealth.Core.Services;
using ApiHealth.Domain;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace ApiHealth.Test;

[TestFixture]
public class HealthServiceTests
{
    private Mock<IHealthDataBase>? _mockHealthCore;
    private HealthServiceCore? _healthService;

    [SetUp]
    public void Setup()
    {
        _mockHealthCore = new Mock<IHealthDataBase>();
        _healthService = new HealthServiceCore(_mockHealthCore.Object);
    }

    [Test]
    public void TestBuscarServicioPorNombre()
    {
        // Configurar objeto simulado de IHealthDataBase
        var mockHealthDataBase = new Mock<IHealthDataBase>();
        mockHealthDataBase.Setup(h => h.GetHealthServiceByName("Usuarios"))
                          .Returns(new ResponseServices 
                          {
                              Name = "Usuarios",
                              EndPoint = "http://localhost:8080/health",
                              Emails = "correo@gmail.com"
                          });

        // Crear instancia de HealthServiceCore con el objeto simulado
        var healthService = new HealthServiceCore(mockHealthDataBase.Object);

        // Ejecutar el método que se va a probar
        var result = healthService.GetHealthServiceByName("Usuarios");

        // Verificar el resultado
        ClassicAssert.IsNotNull(result);
        // Agregar más aserciones según sea necesario
    }

    [Test]
    public void TestBuscarServicioPorNombreErroneo()
    {
        // Configurar objeto simulado de IHealthDataBase
        var mockHealthDataBase = new Mock<IHealthDataBase>();
        mockHealthDataBase.Setup(h => h.GetHealthServiceByName("Usuarios"))
                          .Returns(new ResponseServices
                          {
                              Name = "Usuarios",
                              EndPoint = "http://localhost:8080/health",
                              Emails = "correo@gmail.com"
                          });

        // Crear instancia de HealthServiceCore con el objeto simulado
        var healthService = new HealthServiceCore(mockHealthDataBase.Object);

        // Ejecutar el método que se va a probar
        var result = healthService.GetHealthServiceByName("Users");

        // Verificar el resultado
        ClassicAssert.IsNull(result);
        // Agregar más aserciones según sea necesario
    }

    [Test]
    public void TestParaGuardarServicio()
    {
        // Configurar objeto simulado de IHealthDataBase
        var mockHealthDataBase = new Mock<IHealthDataBase>();
        mockHealthDataBase.Setup(h => h.SaveService(It.IsAny<RequestService>()))
                          .Returns(true);

        var requestService = new RequestService
        {
            Name = "Servicio Test",
            Frequency = 60,
            EndPoint = "http://localhost:8080/health",
            Emails = "correo@ejemplo.com"
        };

        var healthService = new HealthServiceCore(mockHealthDataBase.Object);

        // Ejecutar el método que se va a probar
        var result = healthService.SaveService(requestService);

        // Verificar el resultado
        ClassicAssert.IsTrue(result);
        // Agregar más aserciones según sea necesario
    }
}

