using System;
using System.Collections.Generic;

namespace SystemPlus.Web
{
    public static class BotDetector
    {
        static readonly List<string> bots = new List<string>()
        {
            //"AdsBot-Google",
            "AhrefsBot",
            "Baiduspider",
            //"bingbot",
            "Exabot",
            //"Googlebot",
            "MJ12bot",
            "rogerbot",
            "SemrushBot",
            "SeznamBot",
            "Uptimebot",
            "YandexImages",
        };

        public static bool IsBot(string userAgent)
        {
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                foreach (string bot in bots)
                {
                    if (userAgent.Contains(bot, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }
    }
}
