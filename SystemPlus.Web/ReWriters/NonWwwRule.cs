using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Text;

namespace SystemPlus.Web.ReWriters
{
    /// <summary>
    /// Redirect www.abc.com to abc.com
    /// </summary>
    public class NonWwwRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            HttpRequest req = context.HttpContext.Request;
            HostString currentHost = req.Host;

            if (currentHost.Host.StartsWith("www.", StringComparison.InvariantCulture))
            {
                HostString newHost = new HostString(currentHost.Host.Substring(4), currentHost.Port ?? 80);
                StringBuilder newUrl = new StringBuilder().Append("http://").Append(newHost).Append(req.PathBase).Append(req.Path).Append(req.QueryString);

                context.HttpContext.Response.Redirect(newUrl.ToString(), true);
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}
