using Polly;
using Wpm.Clinic;
using Wpm.Clinic.Application;
using Wpm.Clinic.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddManagmentDbContext();

builder.Services.AddServices();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<ManagementService>(client =>
{
    var uri = builder.Configuration.GetValue<string>("Wpm:ManagementUri");
    client.BaseAddress = new Uri(uri);
}).AddResilienceHandler("management-pipeline", builder =>
{
    builder.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>()
    {
        BackoffType = DelayBackoffType.Exponential,
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(10)
    });
});

var app = builder.Build();


app.EnsureDbIsCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();