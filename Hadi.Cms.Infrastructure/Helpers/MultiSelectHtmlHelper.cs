using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class MultiSelectHtmlHelper
    {
        public static MvcHtmlString MultiSelect<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string url, string type = "json", int maxselectitem = 1, string placeholder = "Select Items ...", string autoBind = "false", string serverfiltering = "true")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, helper.ViewData);
            var name = metadata.PropertyName;
            var value = metadata.Model;

            string showWhat = @"";
            string multiSelectHtml = @"
                    <div class='k-multiselect'><select id='" + name + @"' multiple='multiple' name='" + name + @"'>
                    </select></div>";
            string multiSelectJs = @"
                    	<script>
		                    $(function () {
		                    $('#" + name + @"').kendoMultiSelect({
		                            maxSelectedItems: " + maxselectitem + @",
				                    placeholder: '" + placeholder + @"',
				                    dataTextField: 'Name',
				                    dataValueField: 'Id',
				                    autoBind: false,
				                    dataSource:
                                    {
                                        type: '" + type + @"',
					                    serverFiltering: " + serverfiltering + @",
										transport:{
											read:
                                                {
													url: '" + url + @"',
						                        }
                                    }
                                }
                            });" +
                            (value == null ? "" : @"$('#" + name + @"').data('kendoMultiSelect').value(['" + value + @"']);") +
                        @"});
            </script>";

            showWhat = multiSelectHtml + multiSelectJs;

            //return MvcHtmlString.Create(showWhat);
            return new MvcHtmlString(showWhat);
        }
    }
}
