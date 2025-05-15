using Microsoft.Extensions.Options;
using Stripe;
using WebAPI_ForTTTN.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin() // Allows any origin (CORS)
              .AllowAnyMethod()  // Allows any HTTP method (GET, POST, etc.)
              .AllowAnyHeader(); // Allows any HTTP header
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


builder.Services.AddSingleton<StripeClient>(sp =>
{
    var stripeSettings = sp.GetRequiredService<IOptions<StripeSettings>>().Value;
    return new StripeClient(stripeSettings.SecretKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
