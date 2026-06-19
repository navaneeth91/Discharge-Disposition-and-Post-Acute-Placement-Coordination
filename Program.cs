using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Hospital.Data;
using DischargeDisposition_Backend.Insurance.Data;

var builder = WebApplication.CreateBuilder(args);

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