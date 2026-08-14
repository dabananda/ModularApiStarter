using Microsoft.OpenApi.Models;
using ModularApiStarter.Modules.Greeting;
using ModularApiStarter.Shared;
using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;
using ModularApiStarter.Shared.Middlewares;
using Serilog;

// Bootstrap logger: catches any startup failures before the host/config is built.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up ModularApiStarter");

    var builder = WebApplication.CreateBuilder(args);

    // Replace the bootstrap logger with the fully configured one, driven by appsettings ("Serilog" section).
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter 'Bearer' [space] and then your token. Example: \"Bearer eyJhbGciOi...\"",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    });

    builder.Services.Configure<AppSettings>(builder.Configuration);
    builder.Services.AddSharedDI();

    // per-module registrations go here, e.g.:
    // builder.Services.AddRequestHandlers(typeof(LinkModuleMarker).Assembly);
    // builder.Services.AddValidators(typeof(LinkModuleMarker).Assembly);
    // (GreetingModule below handles its own registration inside RegisterModule)

    var modules = new IModule[] { new GreetingModule() /* , new YourModule(), ... */ };

    foreach (var module in modules)
        module.RegisterModule(builder.Services, builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    foreach (var module in modules)
        module.MapEndpoints(app);

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "ModularApiStarter terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}