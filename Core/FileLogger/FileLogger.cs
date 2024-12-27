using System.Text;

namespace Transactions_Web_API.Core.FileLogger
{
  public class FileLogger : ILogger, IDisposable
  {
    string filePath;
    static object _lock = new object();
    public FileLogger(string path)
    {
      filePath = path;
    }
    public IDisposable BeginScope<TState>(TState state)
    {
      return this;
    }

    public void Dispose() { }

    public bool IsEnabled(LogLevel logLevel)
    {
      return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId,
                TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
      lock (_lock)
      {
        StringBuilder sB = new StringBuilder();
        sB.Append(DateTime.Now.ToString()).Append(". ").Append(formatter(state, exception)).Append(Environment.NewLine);
        File.AppendAllText(filePath, sB.ToString());
      }
    }
  }
}
