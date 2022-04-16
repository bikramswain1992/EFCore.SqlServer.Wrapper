using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SerilogLib;
public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        string serilogSettings = configuration.GetSection("Serilog").Value;
        if (serilogSettings != null)
        {
            throw new ArgumentNullException("Could not find serilog settings");
        }

        // Full setup of serilog. Read log settings from appsettings.json
        builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
    }

    public static void AddSerilog(this WebApplication app, string template = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms")
    {
        app.UseSerilogRequestLogging(configure =>
        {
            configure.MessageTemplate = template;
        });
    }
}
