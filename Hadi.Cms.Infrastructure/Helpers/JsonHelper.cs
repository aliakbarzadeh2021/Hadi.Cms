using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        public static MvcHtmlString HiddenJsonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return HiddenJsonFor(htmlHelper, expression, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString HiddenJsonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return HiddenJsonFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        public static MvcHtmlString HiddenJsonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return InternalHiddenJson(htmlAttributes, name, metadata.Model);
        }

        public static MvcHtmlString HiddenJson(this HtmlHelper htmlHelper, string name, object value)
        {
            return InternalHiddenJson(null, name, value);
        }
        public static MvcHtmlString HiddenJson(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            return InternalHiddenJson(htmlAttributes, name, value);
        }
        public static MvcHtmlString HiddenJson(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return InternalHiddenJson(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), name, value);
        }

        private static MvcHtmlString InternalHiddenJson(IDictionary<string, object> htmlAttributes, string name, object value)
        {
            var tagBuilder = new TagBuilder("input");

            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);

            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("type", "hidden");

            var json = JsonConvert.SerializeObject(value);

            tagBuilder.MergeAttribute("value", json);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}
