using Serilog;
using Serilog.Events;

namespace SerilogLib;
public static class SerilogPreConfig
{
    static SerilogPreConfig()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    public static void InitiateSerilog()
    {
        Log.Information("Starting application...");
    }

    public static void LogError(Exception ex,string errorMessage)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
    }

    public static void CloseSerilog()
    {
        Log.CloseAndFlush();
    }
}
