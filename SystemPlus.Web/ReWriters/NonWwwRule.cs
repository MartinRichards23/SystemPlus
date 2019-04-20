using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
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
            var req = context.HttpContext.Request;
            var currentHost = req.Host;
            if (currentHost.Host.StartsWith("www."))
            {
                var newHost = new HostString(currentHost.Host.Substring(4), currentHost.Port ?? 80);
                var newUrl = new StringBuilder().Append("http://").Append(newHost).Append(req.PathBase).Append(req.Path).Append(req.QueryString);
                context.HttpContext.Response.Redirect(newUrl.ToString(), true);
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}
