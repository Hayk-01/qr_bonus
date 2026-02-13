using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;
using QRBonus.BLL;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Middlewares;
using QRBonus.BLL.Services.UserService;
using QRBonus.DAL;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new[] { new Uri(builder.Configuration["ElasticConfiguration:Uri"]!) }, opts =>
    {
        opts.DataStream = new DataStreamName("logs", builder.Configuration["ElasticConfiguration:DataSet"]!,
            builder.Configuration["ElasticConfiguration:Namespace"]!);
        opts.BootstrapMethod = BootstrapMethod.Failure;
        opts.ConfigureChannel = channelOpts =>
        {
            channelOpts.BufferOptions = new BufferOptions
            {
                ExportMaxConcurrency = 10
            };
        };
    },
        transport =>
        {
            transport.Authentication(new BasicAuthentication(builder.Configuration["ElasticConfiguration:Username"]!,
                builder.Configuration["ElasticConfiguration:Password"]!));
        })
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .CreateBootstrapLogger();

try
{

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .MinimumLevel.Error()
        .MinimumLevel.Override("QRBonus", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new[] { new Uri(builder.Configuration["ElasticConfiguration:Uri"]!) }, opts =>
        {
            opts.DataStream = new DataStreamName("logs", builder.Configuration["ElasticConfiguration:DataSet"]!,
                builder.Configuration["ElasticConfiguration:Namespace"]!);
            opts.BootstrapMethod = BootstrapMethod.Failure;
            opts.ConfigureChannel = channelOpts =>
            {
                channelOpts.BufferOptions = new BufferOptions
                {
                    ExportMaxConcurrency = 10
                };
            };
        },
            transport =>
            {
                transport.Authentication(new BasicAuthentication(
                    builder.Configuration["ElasticConfiguration:Username"]!,
                    builder.Configuration["ElasticConfiguration:Password"]!));
            })
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName));

    // Add services to the container.
    builder.Services.AddCors();

    builder.Services.AddDbContext(builder.Configuration);

#if !DEBUG
    builder.Services.AddHangfire(configuration =>
        configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSerilogLogProvider()
            .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("QrBonus")),
                new PostgreSqlStorageOptions
                {
                    DistributedLockTimeout = TimeSpan.FromMinutes(5)
                }));
    builder.Services.AddHangfireServer();
#endif


    builder.Services.AddAuthentication("UserAuth")
        .AddScheme<AuthenticationSchemeOptions, UserAuthenticationHandler>("UserAuth", null);

    builder.Services.RegisterAdminServices(builder.Configuration);

    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "ApiKey must appear in header",
            Type = SecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"           
        });
        var key = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            },
            In = ParameterLocation.Header
        };
        var requirement = new OpenApiSecurityRequirement
        {
            { key, new List<string>() }
        };
        c.AddSecurityRequirement(requirement);
    });

    var app = builder.Build();

    await app.DatabaseMigrate();

#if !DEBUG
    await app.AddBackgroundJobs();
#endif
    
    app.UseCors(x =>
    {
        x.SetIsOriginAllowed(_ => true);
        x.AllowAnyMethod();
        x.AllowAnyHeader();
        x.AllowCredentials();
        x.WithExposedHeaders("Content-Disposition");
    });

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

#if !DEBUG
    app.UseHangfireDashboard(options: new DashboardOptions()
    {
        Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                User = builder.Configuration.GetSection("HangfireSettings:UserName").Value,
                Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
            }
        }
    });
#endif

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(builder.Configuration["FileSettings:FilePath"]!),
        RequestPath = builder.Configuration["FileSettings:RequestPath"]
    });
    
    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<LanguageMiddleware>();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}