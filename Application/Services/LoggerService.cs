using Transactions_Web_API.Application.Interfaces;

namespace Transactions_Web_API.Application.Services
{
  public class LoggerService : ILoggerService
  {
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
      _logger = logger;
    }

    public void LogInformation(string message)
    {
      _logger.LogInformation(message);
    }

    public void LogError(string message)
    {
      _logger.LogError(message);
    }
  }
}
