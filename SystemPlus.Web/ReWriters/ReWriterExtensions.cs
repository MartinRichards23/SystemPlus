using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPlus.Web.ReWriters
{
    public static class ReWriterExtensions
    {
        public static IApplicationBuilder UseWwwRedirection(this IApplicationBuilder app)
        {
            var options = new RewriteOptions();
            options.Rules.Add(new NonWwwRule());
            app.UseRewriter(options);

            return app;
        }
    }
}
