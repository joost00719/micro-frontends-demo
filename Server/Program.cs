using Microsoft.Extensions.FileProviders;
using RPBlazorPlugin.Loader;

namespace Server
{
    public class Program
    {
        private const string nugetPackageTestPath = @"C:\Users\Joost\source\repos\micro-frontends-demo\StandAloneService\bin\Debug\StandAloneService.1.0.0.nupkg";
        private static WebApplication _app;

        public static async Task Main(string[] args)
        {
            await RunServerAsync(args);
        }

        private static async Task RunServerAsync(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddPlugins(new PluginManagerSettings
            {
                PackedPluginsFolderPath = @"C:\Users\Joost\source\repos\micro-frontends-demo\StandAloneService\bin\Debug",
                WWWRootFolderPath = builder.Environment.WebRootPath!
            });

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            _app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!_app.Environment.IsDevelopment())
            {
                _app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _app.UseHsts();
            }

            _app.UseHttpsRedirection();

            _app.UseStaticFiles();
            _app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/_content/Server",
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath)),
                ServeUnknownFileTypes = true // serve extensionless files
            });

            _app.UseRouting();

            _app.MapBlazorHub();
            _app.MapFallbackToPage("/_Host");

            _app.Run();
        }
    }
}
//           