using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Spix.AppBack.Data;
using Spix.Infrastructure;
using Spix.Services.ImplemenEntities;
using Spix.Services.InterfacesEntities;
using Spix.UnitOfWork.ImplemenEntities;
using Spix.UnitOfWork.InterfacesEntities;
using Spix.Helper.Transactions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Spix.AppBack.LoadCountries;

var builder = WebApplication.CreateBuilder(args);

// Para Controlar las Referencias Cíclicas
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddOpenApi();

// Configuración de Versionado de API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configuración de Swagger para reconocer las versiones correctamente
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend - V1", Version = "1.0" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Orders Backend - V2", Version = "2.0" });

    options.DocInclusionPredicate((version, desc) =>
    {
        var versions = desc.ActionDescriptor.EndpointMetadata.OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
        return versions.Any(v => $"v{v.MajorVersion}" == version);
    });

    options.CustomSchemaIds(type => type.Name.Replace("Controller", "").Replace("V", ""));

    // Agregar seguridad con Bearer Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.<br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br />
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // Filtro para eliminar el parámetro de versión en Swagger UI
    options.OperationFilter<RemoveVersionParameterFilter>();
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7175")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
    });
});

// Configuración de Base de Datos
builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer("name=DefaultConnection", option => option.MigrationsAssembly("Spix.AppBack")));

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ITransactionManager, TransactionManager>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();
builder.Services.AddScoped<IStatesService, StatesService>();
builder.Services.AddScoped<ICityUnitOfWork, CityUnitOfWork>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ISoftPlanUnitOfWork, SoftPlanUnitOfWork>();
builder.Services.AddScoped<ISoftPlanService, SoftPlanService>();

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

// Configuración del HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders Backend - V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Orders Backend - V2");
    });

    string swaggerUrl = "https://localhost:7224/swagger";
    Task.Run(() => OpenBrowser(swaggerUrl));
}

// Activación de CORS
app.UseCors("AllowSpecificOrigin");

// Configuración para servir archivos estáticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "Images")
    ),
    RequestPath = "/Images"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Método para abrir el navegador automáticamente
static void OpenBrowser(string url)
{
    try
    {
        var psi = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al abrir el navegador: {ex.Message}");
    }
}

// Filtro para eliminar el parámetro de versión en Swagger UI
public class RemoveVersionParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParam = operation.Parameters?.FirstOrDefault(p => p.Name == "version");
        if (versionParam != null)
        {
            operation.Parameters!.Remove(versionParam);
        }
    }
}