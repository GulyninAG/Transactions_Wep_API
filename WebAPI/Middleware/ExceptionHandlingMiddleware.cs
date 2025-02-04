using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Transactions_Web_API.Application.Interfaces;

namespace Transactions_Web_API.WebAPI.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Произошла ошибка: {Message}", ex.Message);
        await HandleExceptionAsync(context, ex);
      }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/problem+json"; // Указываем тип контента для RFC 9457

      var problemDetails = new ProblemDetails
      {
        Type = "https://tools.ietf.org/html/rfc9457", // Тип ошибки
        Title = "Произошла ошибка", // Заголовок ошибки
        Status = (int)HttpStatusCode.InternalServerError, // Статус код по умолчанию
        Detail = exception.Message, // Подробное описание ошибки
        Instance = context.Request.Path // URI запроса
      };

      // Настройка статус кода и заголовка для специфических исключений
      switch (exception)
      {
        case ValidationException validationException:
          problemDetails.Status = (int)HttpStatusCode.BadRequest;
          problemDetails.Title = "Ошибка валидации";
          problemDetails.Detail = "Одна или несколько ошибок валидации произошли.";
          problemDetails.Extensions["errors"] = validationException.Errors; // Дополнительные данные
          break;

        case KeyNotFoundException:
          problemDetails.Status = (int)HttpStatusCode.NotFound;
          problemDetails.Title = "Ресурс не найден";
          problemDetails.Detail = "Запрошенный ресурс не найден.";
          break;

        case UnauthorizedAccessException:
          problemDetails.Status = (int)HttpStatusCode.Unauthorized;
          problemDetails.Title = "Доступ запрещен";
          problemDetails.Detail = "У вас нет прав для выполнения этого действия.";
          break;

        case ArgumentException:
          problemDetails.Status = (int)HttpStatusCode.BadRequest;
          problemDetails.Title = "Неверный аргумент";
          problemDetails.Detail = "Один или несколько аргументов неверны.";
          break;
      }

      context.Response.StatusCode = problemDetails.Status;

      // Сериализация ответа в JSON
      var jsonOptions = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
      };

      var jsonResponse = JsonSerializer.Serialize(problemDetails, jsonOptions);
      await context.Response.WriteAsync(jsonResponse);
    }
  }

  // Класс для представления Problem Details
  public class ProblemDetails
  {
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
  }
}
