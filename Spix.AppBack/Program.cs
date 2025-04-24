using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Spix.AppBack.Data;
using Spix.Infrastructure;
using Spix.Services.ImplementEntities;
using Spix.Services.InterfacesEntities;
using Spix.UnitOfWork.ImplementEntities;
using Spix.UnitOfWork.InterfacesEntities;
using Spix.Helper.Transactions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Spix.AppBack.LoadCountries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Spix.CoreShared.ResponsesSec;
using AppUser = Spix.Core.Entities.User;
using Spix.Helper;
using Spix.UnitOfWork.InterfacesSecure;
using Spix.UnitOfWork.ImplementSecure;
using Spix.Services.InterfacesSecure;
using Spix.Services.ImplementSecure;

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

// Configuración de Base de Datos
builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer("name=DefaultConnection", option => option.MigrationsAssembly("Spix.AppBack")));

//Para realizar logueo de los usuarios
builder.Services.AddIdentity<AppUser, IdentityRole>(cfg =>
{
    //Agregamos Validar Correo para dar de alta al Usuario
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;

    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //Sistema para bloquear por 5 minutos al usuario por intento fallido
    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  //TODO: Cambiar Tiempo de Bloqueo a Usuarios
    cfg.Lockout.AllowedForNewUsers = true;
}).AddDefaultTokenProviders()  //Complemento Validar Correo
  .AddEntityFrameworkStores<DataContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

//Configuracion de la Clase SendGridSetting para transportar los valores del AppSetting
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
//Configuracion de la Clase ImgSetting para transportar los valores del AppSetting
builder.Services.Configure<ImgSetting>(builder.Configuration.GetSection("ImgSoftware"));
//Configuracion de la Clase ImgSetting para transportar los valores del AppSetting
builder.Services.Configure<JwtKeySetting>(options =>
{
    options.jwtKey = builder.Configuration.GetValue<string>("jwtKey");
});

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IUtilityTools, UtilityTools>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
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
builder.Services.AddScoped<ICorporationUnitOfWork, CorporationUnitOfWork>();
builder.Services.AddScoped<ICorporationService, CorporationService>();
builder.Services.AddScoped<IManagerUnitOfWork, ManagerUnitOfWork>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IAccountUnitOfWork, AccountUnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();

string? frontUrl = builder.Configuration["UrlFrontend"]; //Se tomta la UrlBlazor desde Appsetting.
// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins(frontUrl!)
     .AllowAnyHeader()
     .AllowAnyMethod()
     .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
    });
});

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