
using Microsoft.EntityFrameworkCore;
using Transactions_Web_API.Core;
using Transactions_Web_API.Core.FileLogger;

/*List<Transactions_Web_API.Models.Transaction> transactions = new List<Transactions_Web_API.Models.Transaction>
{
    new() { Id = new Guid(), TransactionDate = DateTime.Now, Amount = 100.25M },
    new() { Id = new Guid(), TransactionDate = DateTime.Now.AddMinutes(1), Amount = 101.75M },
    new() { Id = new Guid(), TransactionDate = DateTime.Now.AddMinutes(2), Amount = 125.65M }
};*/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddDbContext<TransactionDBContext>();

builder.Services.AddProblemDetails();

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapGet("/api/v1/Transactions", async (TransactionDBContext dB) => await dB.Transactions.ToListAsync());

app.MapGet("/api/v1/Transaction", async (string id, ILogger<FileLogger> logger, TransactionDBContext dB) =>
{
  if (Guid.TryParse(id, out Guid guid))
  {
    Transactions_Web_API.Models.Transaction? transaction = await dB.Transactions.FirstOrDefaultAsync(t => t.Id.ToString() == id);
    if (transaction == null)
    {
      logger.LogInformation("���������� �� �������.");
      throw new Exception("���������� �� �������.");
    }
    return Results.Ok(transaction);
  }
  else
  {
    logger.LogInformation("�������� ������ ID.");
    throw new Exception("�������� ������ ID.");
  }
});

app.MapPost("/api/v1/Transaction", async Task<IResult>(Transactions_Web_API.Models.Transaction transaction, ILogger<FileLogger> logger, TransactionDBContext dB) => 
{
  if (await dB.Transactions.AnyAsync(t => t.Id == transaction.Id)) return Results.Json(transaction);

  if (await dB.Transactions.CountAsync() >= 100)
  {
    logger.LogInformation("��������� ������������ ���������� ���������� (100).");
    throw new Exception("��������� ������������ ���������� ���������� (100).");
  }

  if (transaction.TransactionDate > DateTime.Now)
  {
    logger.LogInformation("���� �� ����� ���� � �������.");
    throw new Exception("���� �� ����� ���� � �������.");
  }

  if (transaction.Amount <= 0)
  {
    logger.LogInformation("����� ������ ���� �������������.");
    throw new Exception("����� ������ ���� �������������.");
  }

  transaction.InsertDateTime = DateTime.Now;
  await dB.Transactions.AddAsync(transaction);
  await dB.SaveChangesAsync();

  return Results.Json(transaction.InsertDateTime);
});

app.MapDelete("/api/v1/Transaction/Delete/{id}", async (string id, ILogger<FileLogger> logger, TransactionDBContext dB) =>
{
  Transactions_Web_API.Models.Transaction? transaction = await dB.Transactions.FirstOrDefaultAsync(t => t.Id.ToString() == id);

  if (transaction == null)
  {
    logger.LogInformation("���������� �� �������.");
    throw new Exception("���������� �� �������.");
  }

  dB.Transactions.Remove(transaction);
  await dB.SaveChangesAsync();
  return Results.Json(transaction);
});

app.Run();
