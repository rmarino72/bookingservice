using bookingservice.db;
using bookingservice.security;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var level = LogLevel.Trace;
// Add services to the container.

builder.Services.AddAuthentication("TokenAuthentication")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>
        ("TokenAuthentication", null);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });


var loggerFactory = LoggerFactory.Create(loggerFactoryBuilder =>
{
    loggerFactoryBuilder.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
    loggerFactoryBuilder.AddDebug();
    loggerFactoryBuilder.SetMinimumLevel(level);
});
ILogger logger = loggerFactory.CreateLogger("Booking Service");

IDbManager db = new DbManager(logger);

builder.Services.AddSingleton(logger);
builder.Services.AddSingleton(db);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();