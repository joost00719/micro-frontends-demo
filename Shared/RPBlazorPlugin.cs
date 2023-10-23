namespace Shared
{
    public interface IPlugin
    {
        public List<string> Scripts { get; }

        public List<string> StyleSheets { get; }

        public string Name { get; }

        public static abstract IPlugin CreateInstance();
    }
}