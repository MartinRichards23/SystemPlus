namespace SystemPlus.Net
{
    /// <summary>
    /// Maintains a list of some common user agents
    /// </summary>
    public static class UserAgents
    {
        //static readonly IList<UserAgent> agents = new List<UserAgent>();

        //static UserAgents()
        //{
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36", OperatingSystem.Windows, "8.1", Browser.Chrome, "32.0.1667.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36", OperatingSystem.Windows, "8", Browser.Chrome, "32.0.1667.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36", OperatingSystem.Windows, "7", Browser.Chrome, "32.0.1667.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36", OperatingSystem.Windows, "Vista", Browser.Chrome, "32.0.1667.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 5.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36", OperatingSystem.Windows, "XP_64", Browser.Chrome, "32.0.1667.0"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "8.1", Browser.Chrome, "31.0.1650.16"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "8", Browser.Chrome, "31.0.1650.16"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "7", Browser.Chrome, "31.0.1650.16"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "Vista", Browser.Chrome, "31.0.1650.16"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 5.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "XP_64", Browser.Chrome, "31.0.1650.16"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36", OperatingSystem.Windows, "XP", Browser.Chrome, "31.0.1650.16"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1664.3 Safari/537.36", OperatingSystem.OSX, "10_9_0, Intel", Browser.Chrome, "32.0.1664.3"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.3; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0", OperatingSystem.Windows, "8.1", Browser.IExplorer, "10.6"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.2; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0", OperatingSystem.Windows, "8", Browser.IExplorer, "10.6"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0", OperatingSystem.Windows, "7", Browser.IExplorer, "10.6"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.0; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0", OperatingSystem.Windows, "Vista", Browser.IExplorer, "10.6"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.3; WOW64; Trident/6.0)", OperatingSystem.Windows, "8.1", Browser.IExplorer, "10.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)", OperatingSystem.Windows, "8", Browser.IExplorer, "10.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)", OperatingSystem.Windows, "7", Browser.IExplorer, "10.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.0; WOW64; Trident/6.0)", OperatingSystem.Windows, "Vista", Browser.IExplorer, "10.0"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "8.1", Browser.Firefox, "25.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "8", Browser.Firefox, "25.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "7", Browser.Firefox, "25.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.0; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "Vista", Browser.Firefox, "25.0"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "8.1", Browser.Firefox, "28.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "8", Browser.Firefox, "28.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "7", Browser.Firefox, "28.0"));
        //    agents.Add(new UserAgent("Mozilla/5.0 (Windows NT 6.0; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0", OperatingSystem.Windows, "Vista", Browser.Firefox, "28.0"));

        //    agents.Add(new UserAgent("Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:24.0) Gecko/20100101 Firefox/24.0", OperatingSystem.Linux, "Ubuntu with X11", Browser.Firefox, "24.0"));
        //}
        
        public static UserAgent MozillaUserAgent
        {
            get { return new UserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0", OperatingSystem.Windows, "10", Browser.Firefox, "49.0"); }
        }

        //public static IList<UserAgent> Agents
        //{
        //    get { return agents; }
        //}
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