namespace Transactions_Web_API.Application.Interfaces
{
  public interface ILoggerService
  {
    void LogInformation(string message);
    void LogError(string message);
  }
}
