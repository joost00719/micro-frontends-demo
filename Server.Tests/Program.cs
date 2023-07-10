using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ServerProgram = Server.Program;

namespace Server.Tests
{
    [TestClass]
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private static WebApplicationFactory<ServerProgram> _webApplicationFactory;
        private static IServiceScope _serviceScope;

        [AssemblyInitialize()]
        public static async Task AssemblyInit(TestContext context)
        {
            var sc = new ServiceCollection();
            ConfigureServices(sc);
            ServiceProvider = sc.BuildServiceProvider();

            _webApplicationFactory = new WebApplicationFactory<ServerProgram>();
            _serviceScope = _webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        }

        private static void ConfigureServices(IServiceCollection services)
        {
        }

        [AssemblyCleanup()]
        public static async Task AssemblyCleanup()
        {
        }
    }
}