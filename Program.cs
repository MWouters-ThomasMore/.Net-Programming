using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebAPIDemoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection2")));

// Dirty fix: Niet gebruiken op examen
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Register services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure Identity
builder.Services.AddIdentity<CustomUser, IdentityRole>()
    .AddEntityFrameworkStores<WebAPIDemoContext>();
builder.Services.AddTransient<IdentitySeeding>();

// We gebruiken de MySettings in Token Class, de Login methode heeft deze nodig
// Token overpompen in MySettings
Token.mySettings = new MySettings
{
    Secret = (builder.Configuration["MySettings:Secret"] ?? "d4f.5E6a7-8b9c-0d1e-WfGl1m-4h5i6j7k8l9m").ToCharArray(),
    ValidIssuer = builder.Configuration["MySettings:ValidIssuer"] ?? "https://localhost:7055",
    ValidAudience = builder.Configuration["MySettings:ValidAudience"] ?? "https://localhost:7055"
};

var audiences = Token.mySettings.ValidAudiences;

// Authentication toevoegen
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Jwt Bearer toevoegen
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true; // debugging
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.UseSecurityTokenValidators = true; // fix bug 8.0
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = false, // onderdrukken bug 8.0
        //ValidateIssuer = true,
        //ValidateAudience = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidAudiences = audiences,
        //ValidIssuer = Token.mySettings.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Token.mySettings.Secret))
    };
    // Customize the JWT Bearer events
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            // Customize unauthorized response
            context.Response.StatusCode = 401; // Unauthorized
            context.Response.ContentType = "application/json";
            var result = "{\"error\": \"Niet geauthenticeerd. Token is ongeldig of ontbreekt.\"}";
            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            // Customize forbidden response
            context.Response.StatusCode = 403; // Forbidden
            context.Response.ContentType = "application/json";
            var result = "{\"error\": \"Toegang geweigerd. U heeft niet de juiste rechten voor deze actie.\"}";
            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddSwaggerGen(swagger => {
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AuthTest Web API",
        Description = "Authentication and Authorization in ASP.NET with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ZONDER Bearer. Dus gewoon je TOKEN: \r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.GetRequiredSection(nameof(MySettings)).Bind(Token.mySettings);

builder.Services.Configure<IdentityOptions>(options => {
    // Password settings. 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings. 
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

// Plaats code voor builder.Build()
WebApplication app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeding>();
    UserManager<CustomUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();
    // Voeg de rolemanager toe, injecteer deze als parameters naar de seeding klasse.
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await seeder.IdentitySeedingAsync(userManager, roleManager);
}

app.Run();