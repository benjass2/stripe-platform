using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest;
using Microsoft.EntityFrameworkCore;
using PC28130.U202422816.Stripe.Platform.Billing.Application.CommandServices;
using PC28130.U202422816.Stripe.Platform.Billing.Application.Internal.CommandServices;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Repositories;
using PC28130.U202422816.Stripe.Platform.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Repositories;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("The default database connection string is missing.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<IPaymentMandateRepository, PaymentMandateRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaymentMandateCommandService, PaymentMandateCommandService>();

// Add services to the container.

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var messages = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<ApiMessages>>();
        return new BadRequestObjectResult(new ProblemDetails { Status = 400, Title = messages["InvalidRequest"], Detail = messages["InvalidRequest"] });
    };
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures("en", "en-US", "es", "es-PE")
    .AddSupportedUICultures("en", "en-US", "es", "es-PE"));
app.UseExceptionHandler(error => error.Run(async context =>
{
    var messages = context.RequestServices.GetRequiredService<IStringLocalizer<ApiMessages>>();
    context.Response.StatusCode = 500;
    await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = 500, Title = messages["Unexpected"] });
}));
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

