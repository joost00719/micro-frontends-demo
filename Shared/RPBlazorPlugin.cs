namespace Shared
{
    public abstract class AbstractPlugin
    {
        public AbstractPlugin(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
        }

        public List<string> Scripts { get; } = new List<string>();

        public List<string> StyleSheets { get; } = new List<string>();

        public string Name { get; }

        public List<IPluginPageRegistration> Pages { get; } = new List<IPluginPageRegistration>();
    }

    public interface IPluginPageRegistration
    {
        public string GetPageName(string culture);

        public string PagePath { get; }
    }

    public class DefaultPluginPageRegistration : IPluginPageRegistration
    {
        public DefaultPluginPageRegistration(string pagePath, string pageName)
        {
            PagePath = pagePath;
            PageName = pageName;
        }

        public string PagePath { get; }

        public string PageName { get; }

        public string GetPageName(string culture)
        {
            return PageName;
        }
    }
}