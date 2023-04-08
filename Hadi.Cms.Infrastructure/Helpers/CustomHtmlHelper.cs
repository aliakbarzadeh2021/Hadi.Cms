using System;
using System.Text;
using System.Web.Mvc;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class CustomHtmlHelper
    {
        public static MvcHtmlString MetaTag(this HtmlHelper helper, string description, string keywords)
        {
            StringBuilder sbMetaTags = new StringBuilder();

            sbMetaTags.Append("<meta name=\"description\" content =\"" + description + "\">");
            sbMetaTags.Append(Environment.NewLine);
            sbMetaTags.Append("<meta name=\"keywords\" content =\"" + keywords + "\">");

            return new MvcHtmlString(sbMetaTags.ToString());
        }
    }
}
