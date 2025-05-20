using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        var app = builder.Services.BuildServiceProvider().GetRequiredService<IApplicationBuilder>();

        app.UseRouting();

        app.UseCors(policy =>
        {
            policy
                .WithOrigins("http://localhost:3000") // Replace with your frontend origin
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseEndpoints(endpoints => { });
    })
    .ConfigureServices(services =>
    {
        services.AddCors(); // 🔥 Must register it here
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
