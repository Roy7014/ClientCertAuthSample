using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.Logging.AzureAppServices;

var builder = WebApplication.CreateBuilder(args);
//ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
//builder.Logging.AddAzureWebAppDiagnostics(); 
//builder.Services.Configure<AzureFileLoggerOptions>(options => 
//{ 
//    options.FileName = "azure-diagnostics-Logger"; 
//    options.FileSizeLimit = 50 * 1024; 
//    options.RetainedFileCountLimit = 5; 
//});


//builder.WebHost.ConfigureKestrel(o =>
//{
//    o.ConfigureHttpsDefaults(opts =>
//    {
//        opts.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
        
//    });
//});



builder.Services.AddControllers();
builder.Services.AddSingleton<CertificateValidation>();


//logger.LogInformation("Reached add cli cert 0.1");
builder.Services.AddAuthentication(
    CertificateAuthenticationDefaults.AuthenticationScheme)
.AddCertificate(options =>
{
    options.AllowedCertificateTypes = CertificateTypes.All;
    options.RevocationMode = X509RevocationMode.NoCheck;
   // logger.LogInformation("Reached add cli cert");
   
    options.Events = new CertificateAuthenticationEvents
    {
        OnCertificateValidated = context =>
        {
     //       logger.LogInformation("Reached add cli cert2.0"+ context.ClientCertificate);

            var validationService = context.HttpContext.RequestServices
                .GetRequiredService<CertificateValidation>();

            if (validationService.ValidateCertificate(context.ClientCertificate))
            {
                var claims = new[]
                {
                        new Claim(
                            ClaimTypes.NameIdentifier,
                            context.ClientCertificate.Subject,
                            ClaimValueTypes.String, context.Options.ClaimsIssuer),
                        new Claim(
                            ClaimTypes.Name,
                            context.ClientCertificate.Subject,
                            ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
            }
            else
            {
                context.Fail("Its Certificate validation failure");
            }

            return Task.CompletedTask;
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
    app.UseSwaggerUI();
app.UseCertificateForwarding();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
