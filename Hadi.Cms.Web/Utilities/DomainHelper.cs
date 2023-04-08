using System;
using System.Web;

namespace Hadi.Cms.Web.Utilities
{
    public static class DomainHelper
    {
        public static string GetDomain(HttpRequestBase request)
        {
            return request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host + (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port);
        }
    }
}