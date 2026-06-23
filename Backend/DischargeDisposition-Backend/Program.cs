using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Repositories;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Repositories;
using DischargeDisposition_Backend.Insurance.Services;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddScoped<IDelayReasonCodeRepository,DelayReasonCodeRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped< IDelayReasonCodeService,DelayReasonCodeService>();


builder.Services.AddScoped<IDispositionTypeRepository,DispositionsTypeRepository>();

builder.Services.AddScoped<IDispositionTypeService,DispositionTypeService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ILengthOfStayRepository, LengthOfStayRepository>();
builder.Services.AddScoped<ILengthOfStayService, LengthOfStayService>();
builder.Services.AddScoped<IReferralRepository , ReferralRepository>();
builder.Services.AddScoped<IReferralService, ReferralService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IInsuranceRepository,InsuranceRepository>();
builder.Services.AddScoped<IDispositionDecisionRepository,DispositionDecisionRepository>();
builder.Services.AddScoped<IDispositionDecisionService,DispositionDecisionService>();
builder.Services.AddScoped<IInsuranceService,InsuranceService>();
builder.Services.AddScoped<IPatientDelayRepository,PatientDelayRepository>();
builder.Services.AddScoped<IPatientDelayService,PatientDelayService>();
builder.Services.AddScoped<IDelayReasonCodeRepository, DelayReasonCodeRepository>();
builder.Services.AddScoped<IDelayReasonCodeService, DelayReasonCodeService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPostAcuteProviderRepository,PostAcuteProviderRepository>();

builder.Services.AddScoped<IPostAcuteProviderService,PostAcuteProviderService>();

builder.Services.AddScoped<IMemberRepository,MemberRepository>();

builder.Services.AddScoped<IAuthorizationRepository,AuthorizationRepository>();

builder.Services.AddScoped<IAuthorizationService,AuthorizationService>();

builder.Services.AddScoped<IWebhookService,WebhookService>();

builder.Services.AddScoped<IInsuranceAuthorizationService,InsuranceAuthorizationService>();

builder.Services.AddHttpClient<IWebhookService,WebhookService>();

builder.Services.AddScoped<IMemberService,MemberService>();
var hospitalConnection =
    builder.Configuration.GetConnectionString("HospitalConnection")
    ?? throw new InvalidOperationException(
        "HospitalConnection string not found.");

var insuranceConnection =
    builder.Configuration.GetConnectionString("InsuranceConnection")
    ?? throw new InvalidOperationException(
        "InsuranceConnection string not found.");

builder.Services.AddDbContext<HospitalDbContext>(options =>
{
    options.UseSqlServer(hospitalConnection, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddDbContext<InsuranceDbContext>(options =>
{
    options.UseSqlServer(insuranceConnection, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer" ,options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("VuePolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DischargeDisposition API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT Token as: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("VuePolicy");
app.UseAuthentication();

app.UseAuthorization();

try
{
    app.MapControllers();
}
catch (ReflectionTypeLoadException ex)
{
    foreach (var e in ex.LoaderExceptions)
    {
        Console.WriteLine(e?.Message);
    }

    throw;
}

app.Run();
