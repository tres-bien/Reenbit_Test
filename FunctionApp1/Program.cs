using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//var host = new HostBuilder()
//    .ConfigureFunctionsWorkerDefaults()
//    .Build();

//host.Run();

//// Program.cs
using Microsoft.Extensions.DependencyInjection;
using MailKit.Net.Smtp;

var builder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<SmtpClient>(); // Register the SmtpClient as a singleton
    });

builder.Build().Run();
