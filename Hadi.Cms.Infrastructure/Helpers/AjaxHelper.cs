using System.Web.Mvc;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class AjaxHelper
    {
        public static MvcHtmlString Ajax(this HtmlHelper helper, string type, string data, string targetid, string returntofunction = "returnFunction")
        {
            string result = string.Empty;
            result += "$(\"#preloader\").css(\"display\", \"block\");";
            result += "var x = document.readyState;";
            result += "console.log(x);";
            result += "$.ajax({";
            result += "type:'" + type + "',";
            result += "url:param,";
            result += "data:ajaxdata,";
            result += "success: function(data){";
            result += "if(data != 'Error'){";
            result += returntofunction + "(data);";
            result += "$('#" + targetid + "').html(data);";
            //result += "debugger;";
            result += "$(\".loader\").fadeOut(\"slow\");";
            result += "}";
            result += "else{";
            result += "";
            result += "}";
            result += "}";
            result += "});";
            return new MvcHtmlString(result);
        }
    }
}
