﻿namespace Transactions_Web_API.Core.FileLogger
{
  public class FileLoggerProvider : ILoggerProvider
  {
    string path;
    public FileLoggerProvider(string path)
    {
      this.path = path;
    }
    public ILogger CreateLogger(string categoryName)
    {
      return new FileLogger(path);
    }

    public void Dispose() { }
  }
}
