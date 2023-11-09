namespace RPBlazorPlugin.Core
{
    public interface IPluginPageInfo
    {
        /// <summary>
        /// Get the name of the page. Note that the implementation possibly can use the Thread.CurrentUICulture to return the correct name.
        /// </summary>
        /// <returns></returns>
        string GetPageName();

        /// <summary>
        /// Gets the path of the page. This is the path that is used in the router.
        /// </summary>
        string PagePath { get; }
    }
}