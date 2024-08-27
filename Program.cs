using Microsoft.EntityFrameworkCore;
using PaymentsApi.Data;
using PaymentsApi.Middlewares;
using PaymentsApi.Repositories;
using PaymentsApi.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RemoteConnection");

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<CreditCardRepository>();
builder.Services.AddScoped<DebitCardRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<TokenRepository>();
builder.Services.AddScoped<ApiKeyRepository>();

builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<DebitCardService>();
builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<AuthorizationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();