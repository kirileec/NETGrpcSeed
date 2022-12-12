using EFCore;
using GrpcService.Services;
using Helper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.RollingFile("./logs/log-{Date}.log")
               .CreateLogger();

var dsn = "";
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseMySql(dsn, ServerVersion.AutoDetect(dsn));
    options.LogTo(Log.Logger.Information, LogLevel.Information, null);
}, ServiceLifetime.Transient);
//»º´æ
builder.Services.AddEasyCaching(options =>
{
    options.UseInMemory("default");
    //options.UseRedis(c =>
    //{
    //    c.DBConfig.KeyPrefix = "DotNetSeed_";
    //    c.DBConfig.Database = 2;
    //    c.DBConfig.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6379));
    //},"redis").UseRedisLock() ;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
GlobalServiceProvider.ServiceProvider = app.Services;
app.Run();
