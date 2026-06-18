using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dsw2026Ej15.Api.Middlewares;

/// <summary>
/// El ExceptionMiddleware intercepta todas las peticiones HTTP que viajan por nuestra API.
/// Si en algún controlador o servicio ocurre un error (Exception) que no fue atrapado con un try-catch,
/// este middleware lo captura aquí arriba, registra el error y le responde al cliente de forma amigable.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    // El RequestDelegate '_next' es el siguiente paso en la "tubería" (pipeline) de nuestra API.
    // ILogger nos permite escribir mensajes en la consola de Visual Studio para nosotros los desarrolladores.
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Este método se ejecuta automáticamente en cada petición que entra a la API
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Intentamos continuar con el flujo normal de la petición
            await _next(context);
        }
        catch (Exception ex)
        {
            // ¡Pánico! Algo falló en algún controlador. Registramos el error en consola para depurarlo:
            _logger.LogError(ex, $"Ocurrió una excepción no controlada: {ex.Message}");

            // Le respondemos al usuario de forma segura y prolija
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 1. Configuramos la respuesta HTTP para que el cliente sepa que es un JSON
        context.Response.ContentType = "application/json";

        // 2. Establecemos el código de estado 500 (Internal Server Error)
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // 3. Diseñamos un objeto anónimo con la estructura del error que verá el cliente
        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Ocurrió un error inesperado en el servidor de la clínica. Por favor, intente más tarde.",
            Detailed = exception.Message // En producción se suele ocultar esto, pero para la facu viene excelente para depurar
        };

        // 4. Convertimos nuestro objeto de C# a un texto formateado en JSON
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);

        // 5. Escribimos la respuesta JSON directamente en el cuerpo de la respuesta HTTP
        return context.Response.WriteAsync(jsonResponse);
    }
}