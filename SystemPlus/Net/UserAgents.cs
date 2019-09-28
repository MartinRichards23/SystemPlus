namespace SystemPlus.Net
{
    /// <summary>
    /// Maintains a list of some common user agents
    /// </summary>
    public static class UserAgents
    {
        public static UserAgent MozillaUserAgent
        {
            get { return new UserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0", OperatingSystem.Windows, "10", Browser.Firefox, "69.0"); }
        }
    }

    public enum Browser
    {
        Chrome,
        Firefox,
        IExplorer
    };

    public enum OperatingSystem
    {
        Windows,
        OSX,
        Linux
    };

    public class UserAgent
    {
        public UserAgent(string agentString, OperatingSystem operatingSystem, string operatingSystemVersion, Browser browser, string browserVersion)
        {
            AgentString = agentString;
            OperatingSystem = operatingSystem;
            OperatingSystemVersion = operatingSystemVersion;
            Browser = browser;
            BrowserVersion = browserVersion;
        }

        public string AgentString { get; private set; }

        public OperatingSystem OperatingSystem { get; private set; }
        public string OperatingSystemVersion { get; private set; }

        public Browser Browser { get; private set; }
        public string BrowserVersion { get; private set; }

        public override string ToString()
        {
            return AgentString;
        }
    }
}