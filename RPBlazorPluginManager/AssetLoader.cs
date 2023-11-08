using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPBlazorPluginManager
{
    public class AssetLoader
    {
        private readonly PackageRepository packageRepo;
        private HttpClient httpClient;
        private DocumentObjectModelInterop domInterop;

        public AssetLoader(PackageRepository packageRepo, HttpClient httpClient, DocumentObjectModelInterop domInterop)
        {
            this.packageRepo = packageRepo;
            this.httpClient = httpClient;
            this.domInterop = domInterop;
        }

        public async Task LoadAssets(string baseUrl)
        {
            foreach (var plugin in packageRepo.LoadedPlugins)
            {
                await LoadAssets(baseUrl, plugin);
            }
        }

        public async Task LoadAssets(string baseUrl, LoadedPlugin plugin)
        {
            var url = $"{baseUrl}/plugins/{plugin.Name}/assets.xml";
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
