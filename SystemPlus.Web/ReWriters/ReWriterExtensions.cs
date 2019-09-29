using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

namespace SystemPlus.Web.ReWriters
{
    public static class ReWriterExtensions
    {
        public static IApplicationBuilder UseWwwRedirection(this IApplicationBuilder app)
        {
            RewriteOptions options = new RewriteOptions();
            options.Rules.Add(new NonWwwRule());
            app.UseRewriter(options);

            return app;
        }
    }
}
