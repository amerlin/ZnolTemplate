using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using ZnolBe.BusinessLayer.MapperProfile;
using ZnolBe.BusinessLayer.Services;
using ZnolBe.BusinessLayer.Validations;
using ZnolBe.DataAccessLayer;
using Serilog;
using ZnolBe.WebApi.Swagger;
using Smash.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZnolBe.Shared.Models.Auth;
using ZnolBe.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

const string CorsDomainSectionName = "CorsDomain";
const string CorsPolicyName = "AppCORSPolicy";

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }    
    );

builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<DefaultResponseOperationFilter>();
    option.OperationFilter<AuthResponseOperationFilter>();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ZnolBe API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        });

    option.MapType<DateTime>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date-time",
        Example = new OpenApiString(new DateTime(2023, 04, 08, 22, 0, 0).ToString("yyyy-MM-ddTHH:mm:ssZ"))
    });

    option.UseAllOfToExtendReferenceSchemas();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);
})
.AddFluentValidationRulesToSwagger(options =>
{
    options.SetNotNullableIfMinLengthGreaterThenZero = true;
});

builder.Services
    .AddDbContext<DataContext>(
      options =>
      {
          _ = options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
          _ = options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? string.Empty, c =>
         {
             _ = c.MigrationsAssembly("ZnolBe.DataAccessLayer");
             _ = c.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
             _ = c.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
         });
      });

builder.Services.AddScoped<IDataContext>(services => services.GetRequiredService<DataContext>());

builder.Services.AddAutoMapper(typeof(PersonMapperProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<SaveOrderRequestValidator>();

builder.Services.Scan(scan=>scan.FromAssemblyOf<PeopleService>()
    .AddClasses(classes=>classes.InNamespaceOf<PeopleService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

var jwtConfiguration = builder.Configuration.GetSection("JWT:Config").Get<JwtOptionsConfig>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = jwtConfiguration!.ValidateIssuer,
            ValidateAudience = jwtConfiguration!.ValidateAudience,
            ValidateLifetime = jwtConfiguration!.ValidateLifetime,
            ValidateIssuerSigningKey = jwtConfiguration!.ValidateIssuerSigningKey,
            ValidIssuer = jwtConfiguration!.ValidIssuer,
            ValidAudience = jwtConfiguration!.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration!.Secret)
            ),
        };
    });

builder.Services
    .AddIdentityCore<ZnolBeApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddProblemDetails();

var corsDomains = builder.Configuration[CorsDomainSectionName] ?? string.Empty;
var domains = corsDomains.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
builder.Services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
{
    _ = builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           .WithOrigins(domains);
}));

var app = builder.Build();

UpdateDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZnolBe API V1");
    });
}

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
}