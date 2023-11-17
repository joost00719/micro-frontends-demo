namespace RPBlazorPlugin.Core
{
    /// <inheritdoc/>
    public class DefaultPluginPageInfo : IPluginPageInfo
    {
        public DefaultPluginPageInfo(string pagePath, string pageName)
        {
            PagePath = pagePath;
            PageName = pageName;
        }

        /// <inheritdoc/>
        public string PagePath { get; }

        /// <inheritdoc/>
        public string PageName { get; }

        /// <inheritdoc/>
        public string GetPageName()
        {
            return PageName;
        }
    }
}