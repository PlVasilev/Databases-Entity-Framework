using System;
using AutomapperMyApp.Core;
using AutomapperMyApp.Core.Contracts;
using AutomapperMyApp.Data;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MyApp
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //service container 
            IServiceProvider services = ConfugureSurvuces();
            IEngine engine = new Engine(services);
            engine.Run();
        }

            
        private static IServiceProvider ConfugureSurvuces() // inversion of control container - all depandances should be added of here 
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<MyAppContext>(db => db.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>(); // transient creates new instance every time Mapper Is Called
            serviceCollection.AddTransient<Mapper>();
            // serviceCollection.AddScoped<Mapper>(); // AddScoped creates new instance in the quarry(заявка)
            // serviceCollection.AddSingleton<Mapper>(); // AddSingleton creates new instance in the session (UserSession)

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
