using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPBlazorPluginManager
{
    public class PluginAssetLoader
    {
        private readonly PackageRepository packageRepo;
        private HttpClient httpClient;
        private DocumentObjectModelInterop domInterop;
        private readonly NavigationManager navigationManager;

        public PluginAssetLoader(PackageRepository packageRepo, HttpClient httpClient, DocumentObjectModelInterop domInterop, NavigationManager navigationManager)
        {
            this.packageRepo = packageRepo;
            this.httpClient = httpClient;
            this.domInterop = domInterop;
            this.navigationManager = navigationManager;
        }

        public Assembly[] GetLoadedAssemblies()
        {
            return packageRepo.LoadedPlugins.Select(p => p.Assembly).ToArray();
        }

        public async Task LoadAssets()
        {
            foreach (var plugin in packageRepo.LoadedPlugins)
            {
                await LoadAssets(plugin);
            }
        }

        public async Task LoadAssets(PluginInfo plugin)
        {
            var url = $"{navigationManager.BaseUri}/plugins/{plugin.Name}/assets.xml";
            var stream3 = await httpClient.GetStreamAsync(url);

            XmlDocument assetsList = new XmlDocument();
            assetsList.Load(stream3);

            foreach (XmlNode asset in assetsList.GetElementsByTagName("StaticWebAsset"))
            {
                var content = asset.SelectSingleNode("RelativePath")?.InnerText!;
                if (content.EndsWith(".js"))
                {
                    await domInterop.IncludeScript($"/plugins/{plugin.Name}/{content}");
                }
                else if (content.EndsWith(".css"))
                {
                    await domInterop.IncludeStylesheet($"/plugins/{plugin.Name}/{content}");
                }
            }

        }
    }
}
