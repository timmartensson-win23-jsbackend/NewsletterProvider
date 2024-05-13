using Data.Contexts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SqlServer")));
    })
    .Build();

//using (var scope = host.Services.CreateScope())            //onödigt? Hans pratar om detta i skicka och vaildera verifikationskoder (med servicebus) om det inte går att göra update databas med Enviroment
//{
//    try
//    {
//        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//        var migration = context.Database.GetPendingMigrations();
//        if (migration != null && migration.Any())
//        {
//            context.Database.Migrate();
//        }
//    }
//    catch (Exception ex)
//    {
//        Debug.WriteLine($"Error : Program, Migration of database :: { ex.Message}");
//    }  
//}

host.Run();
