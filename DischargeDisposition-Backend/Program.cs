using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Repositories;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Hospital.Services;
using DischargeDisposition_Backend.Hospital.DTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IReferralRepository , ReferralRepository>();
builder.Services.AddScoped<IReferralService, ReferralService>();
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

// Register Admin Service
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();