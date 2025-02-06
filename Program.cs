using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Transactions_Web_API.Application.Interfaces;
using Transactions_Web_API.Application.Services;
using Transactions_Web_API.Application.Validators;
using Transactions_Web_API.Domain;
using Transactions_Web_API.Infrastructure.FileLogger;
using Transactions_Web_API.Infrastructure.Interfaces;
using Transactions_Web_API.Infrastructure.Services;
using Transactions_Web_API.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransactionDBContext>();
builder.Services.AddProblemDetails();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

// Регистрация сервисов
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IValidator<Transaction>, TransactionValidator>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", policy =>
  {
    policy.AllowAnyOrigin() 
          .AllowAnyMethod() 
          .AllowAnyHeader();
  });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAllOrigins");
app.UseStatusCodePages();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/v1/Transactions", async (ITransactionService transactionService) =>
{
  return await transactionService.GetTransactionsAsync();
});

app.MapGet("/api/v1/Transaction", async (Guid id, ITransactionService transactionService, ILoggerService logger) =>
{
    var transaction = await transactionService.GetTransactionByIdAsync(id);
    if (transaction == null)
    {
      logger.LogInformation("Транзакция не найдена.");
      throw new Exception("Транзакция не найдена.");
    }
    return Results.Json(transaction, statusCode: 200);
});

app.MapPost("/api/v1/Transaction", async (Transaction transaction, ITransactionService transactionService, TransactionDBContext dB, IValidator <Transaction> validator, ILoggerService logger) => 
{
  var validationResult = await validator.ValidateAsync(transaction);
  if (!validationResult.IsValid)
  {
    logger.LogError($"Ошибка валидации: {string.Join(", ", validationResult.Errors)}");
    throw new FluentValidation.ValidationException(validationResult.Errors);
  }

  if (await dB.Transactions.AnyAsync(t => t.Id == transaction.Id))
  {
    return Results.Json(transaction);
  }

  await using var dbContextTransaction = await dB.Database.BeginTransactionAsync();

  try
  {
    // Проверка количества транзакций с блокировкой
    var transactionCount = await dB.Transactions.CountAsync();
    if (transactionCount >= 100)
    {
      logger.LogInformation("Превышено максимальное количество транзакций (100).");
      throw new Exception("Превышено максимальное количество транзакций (100).");
    }

    // Вставка новой транзакции
    await dB.Transactions.AddAsync(transaction);
    await dB.SaveChangesAsync();

    // Фиксация транзакции
    await dbContextTransaction.CommitAsync();

    var insertedObject = new { insertDateTime = DateTime.Now };
    return Results.Json(insertedObject, statusCode: 200);
  }
  catch (Exception ex)
  {
    await dbContextTransaction.RollbackAsync();
    logger.LogError($"Ошибка при добавлении транзакции: {ex.Message}");
    throw;
  }
});

app.MapDelete("/api/v1/Transaction/Delete/{id}", async (Guid id, ITransactionService transactionService, ILoggerService logger) =>
{
  await transactionService.DeleteTransactionAsync(id);
  return Results.Json(new { message = "Транзакция удалена." }, statusCode: 200);
});

app.Run();
