using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MailKit.Net.Smtp;

var builder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<SmtpClient>();
    });

builder.Build().Run();
