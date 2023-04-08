using System;
using System.Web.Mvc;

namespace Hadi.Cms.Web.UI.KendoUI
{
    public static class KendoUIHtmlHelpers
    {
        public static MvcHtmlString KendoGrid(this HtmlHelper helper, string url, KendoGridColumn[] columns, KendoGridCommand[] commands, KendoDataUrlPostData postData = null, bool serverFiltering = false, int pageSize = 20, bool sortable = false, bool resizable = false, string gridId = "grid")
        {
            string dataString = string.Empty;

            if (postData != null && !String.IsNullOrEmpty(postData.InputName))
            {
                dataString = "var InputName = '" + postData.InputName + "';\r\n" +
                "									var value = $('#' + InputName).val();\r\n" +
                "									var result = {{}};\r\n" +
                "									result[InputName] = value;\r\n" +
                "									return result;\r\n";
            }
            else if (postData != null && !String.IsNullOrEmpty(postData.FunctionResult))
            {
                dataString = "return " + postData.FunctionResult + "();";
            }

            string result = string.Format(
                "<div id='{0}'></div>\r\n" +
                "<script>\r\n" +
                "	$('#{0}').kendoGrid({{\r\n" +
                "		height: 450,\r\n" +
                "		dataSource: {{\r\n" +
                "			transport: {{\r\n" +
                "				read: {{\r\n" +
                "					url: '{1}',\r\n" +
                "					dataType: 'json',\r\n" +
                "					type: 'POST',\r\n" +
                "					data: function (){{\r\n" +
                "						" + dataString + "\r\n" +
                "					}}\r\n" +
                "				}}\r\n" +
                "			}},\r\n" +
                "		schema: {{\r\n" +
                "			data: 'data', \r\n" +
                "			total: 'total', \r\n" +
                "			model: {{\r\n" +
                "				fields: {{\r\n"
                , gridId, url);

            foreach (var column in columns)
            {
                string columnType = string.Empty;

                if (column.Type == KendoGridColumnTypes.Number)
                {
                    columnType = "number";
                }
                else if (column.Type == KendoGridColumnTypes.String)
                {
                    columnType = "string";
                }

                result += "					" + column.Name + ": {type: '" + columnType + "'},\r\n";
            }

            result += string.Format(
                "				}}\r\n" +
                "			}}\r\n" +
                "		}},\r\n" +
                "		serverFiltering: " + (serverFiltering ? "true" : "false") + ",\r\n" +
                "		serverPaging: true, \r\n" +
                "		serverSorting: true, \r\n" +
                "		}},\r\n" +//
                "		pageable: {{\r\n" +
                "			pageSize: {0},\r\n" +
                "			pageSizes: [{0}, {0}*2, {0}*4]\r\n" +
                "		}},\r\n" +
                "		sortable: " + (sortable ? "true" : "false") + ",\r\n" +
                "		resizable: " + (resizable ? "true" : "false") + ",\r\n" +
                "		columns: [\r\n", pageSize);

            for (int i = 0; i < columns.Length; i++)
            {
                //string dateFormat = string.Empty;
                string hiddenColumn = string.Empty;

                //if (column.Type == "date")
                //	dateFormat = "format: '{0:MM/dd/yyyy}',\r\n";

                if (columns[i].Hidden == true)
                {
                    hiddenColumn = "			hidden : true,\r\n";
                }

                result +=
                    "		{\r\n" +
                    "			field:'" + columns[i].Name + "',\r\n" +
                    "			title:'" + columns[i].Title + "',\r\n" +
                    //dateFormat +
                    hiddenColumn +
                    "			width: " + columns[i].Width + "\r\n" +
                    "		}";

                if (!(columns.Length == (i + 1) && commands == null && serverFiltering == false))
                {
                    result += ",\r\n";
                }
                else
                {
                    result += "\r\n";
                }
            }

            if (commands != null)
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    result +=
                        "		{\r\n" +
                        "			template: '<a href=\"" + commands[i].Url + "\">" + commands[i].Text + "</a>',\r\n" +
                        "			width: 70\r\n" +
                        "		}";

                    if (commands.Length != (i + 1))
                    {
                        result += ",\r\n";
                    }
                    else
                    {
                        result += "\r\n";
                    }
                }
            }

            result += "		]";

            if (serverFiltering == true)
            {
                result +=
                    ",\r\n" +
                    "		filterable: {\r\n" +
                    "			mode: 'menu',\r\n" +
                    "			extra: false,\r\n" +
                    "			operators: {\r\n" +
                    "				string: {\r\n" +
                    "					contains: kendo.ui.FilterMenu.prototype.options.operators.string.contains,\r\n" +
                    "					doesnotcontain: kendo.ui.FilterMenu.prototype.options.operators.string.doesnotcontain,\r\n" +
                    "				}\r\n" +
                    "			}\r\n" +
                    "		}\r\n";
            }

            result +=
                "	});\r\n" +
                "</script>";

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString KendoGridWithFrozen(this HtmlHelper helper, string url, KendoGridColumn[] columns, KendoGridCommand[] commands, KendoDataUrlPostData postData = null, bool serverFiltering = false, int pageSize = 20, bool sortable = false, bool resizable = false, string gridId = "grid")
        {
            string dataString = string.Empty;

            if (postData != null && !String.IsNullOrEmpty(postData.InputName))
            {
                dataString = "var InputName = '" + postData.InputName + "';\r\n" +
                "									var value = $('#' + InputName).val();\r\n" +
                "									var result = {{}};\r\n" +
                "									result[InputName] = value;\r\n" +
                "									return result;\r\n";
            }
            else if (postData != null && !String.IsNullOrEmpty(postData.FunctionResult))
            {
                dataString = "return " + postData.FunctionResult + "();";
            }

            string result = string.Format(
                "<div id='{0}'></div>\r\n" +
                "<script>\r\n" +
                "	$('#{0}').kendoGrid({{\r\n" +
                "		height: 450,\r\n" +
                "		dataSource: {{\r\n" +
                "			transport: {{\r\n" +
                "				read: {{\r\n" +
                "					url: '{1}',\r\n" +
                "					dataType: 'json',\r\n" +
                "					type: 'POST',\r\n" +
                "					data: function (){{\r\n" +
                "						" + dataString + "\r\n" +
                "					}}\r\n" +
                "				}}\r\n" +
                "			}},\r\n" +
                "		schema: {{\r\n" +
                "			data: 'data', \r\n" +
                "			total: 'total', \r\n" +
                "			model: {{\r\n" +
                "				fields: {{\r\n"
                , gridId, url);

            foreach (var column in columns)
            {
                string columnType = string.Empty;

                if (column.Type == KendoGridColumnTypes.Number)
                {
                    columnType = "number";
                }
                else if (column.Type == KendoGridColumnTypes.String)
                {
                    columnType = "string";
                }

                result += "					" + column.Name + ": {type: '" + columnType + "'},\r\n";
            }

            result += string.Format(
                "				}}\r\n" +
                "			}}\r\n" +
                "		}},\r\n" +
                "		serverFiltering: " + (serverFiltering ? "true" : "false") + ",\r\n" +
                "		serverPaging: true, \r\n" +
                "		serverSorting: true, \r\n" +
                "		}},\r\n" +//
                "		pageable: {{\r\n" +
                "			pageSize: {0},\r\n" +
                "			pageSizes: [{0}, {0}*2, {0}*4]\r\n" +
                "		}},\r\n" +
                "		sortable: " + (sortable ? "true" : "false") + ",\r\n" +
                "		resizable: " + (resizable ? "true" : "false") + ",\r\n" +
                "		columns: [\r\n", pageSize);

            for (int i = 0; i < columns.Length; i++)
            {
                //string dateFormat = string.Empty;
                string hiddenColumn = string.Empty;
                string FrozenColumn = string.Empty;

                //if (column.Type == "date")
                //	dateFormat = "format: '{0:MM/dd/yyyy}',\r\n";

                if (columns[i].Hidden == true)
                {
                    hiddenColumn = "			hidden : true,\r\n";
                }
                if (columns[i].Frozen == true)
                {
                    FrozenColumn = "			locked : true,\r\n";
                }

                result +=
                    "		{\r\n" +
                    "			field:'" + columns[i].Name + "',\r\n" +
                    "			title:'" + columns[i].Title + "',\r\n" +
                    //dateFormat +
                    hiddenColumn +
                    FrozenColumn +
                    "			width: " + columns[i].Width + "\r\n" +
                    "		}";

                if (!(columns.Length == (i + 1) && commands == null && serverFiltering == false))
                {
                    result += ",\r\n";
                }
                else
                {
                    result += "\r\n";
                }
            }

            if (commands != null)
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    string FrozenColumn = string.Empty;
                    if (commands[i].Frozen == true)
                    {
                        FrozenColumn = "			locked : true,\r\n";
                    }

                    result +=
                        "		{\r\n" +
                        "			template: '<a href=\"" + commands[i].Url + "\">" + commands[i].Text + "</a>',\r\n" +
                        FrozenColumn +
                        "			width: 70,\r\n" +
                        "		}";

                    if (commands.Length != (i + 1))
                    {
                        result += ",\r\n";
                    }
                    else
                    {
                        result += "\r\n";
                    }
                }
            }

            result += "		]";

            if (serverFiltering == true)
            {
                result +=
                    ",\r\n" +
                    "		filterable: {\r\n" +
                    "			mode: 'menu',\r\n" +
                    "			extra: false,\r\n" +
                    "			operators: {\r\n" +
                    "				string: {\r\n" +
                    "					contains: kendo.ui.FilterMenu.prototype.options.operators.string.contains,\r\n" +
                    "					doesnotcontain: kendo.ui.FilterMenu.prototype.options.operators.string.doesnotcontain,\r\n" +
                    "				}\r\n" +
                    "			}\r\n" +
                    "		}\r\n";
            }

            result +=
                "	});\r\n" +
                "</script>";

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString KendoGridTree(this HtmlHelper helper, string urlParent, string urlChild, KendoGridColumn[] columnsParent, KendoGridColumn[] columnsChild, KendoGridCommand[] commandsParent, KendoGridCommand[] commandsChild, string filterColumnId, KendoDataUrlPostData postData = null, bool serverFiltering = false, int pageSize = 20, bool sortable = false, bool resizable = false)
        {
            string gridId = "grid";
            string dataString = string.Empty;

            if (postData != null && !String.IsNullOrEmpty(postData.InputName))
            {
                dataString = "var InputName = '" + postData.InputName + "';\r\n" +
                "									var value = $('#' + InputName).val();\r\n" +
                "									var result = {{}};\r\n" +
                "									result[InputName] = value;\r\n" +
                "									return result;\r\n";
            }
            else if (postData != null && !String.IsNullOrEmpty(postData.FunctionResult))
            {
                dataString = "return " + postData.FunctionResult + "();";
            }

            string result = string.Format(
                "<div id='{0}'></div>\r\n" +
                "<script>\r\n" +
                "	$('#{0}').kendoGrid({{\r\n" +
                "		height: 450,\r\n" +
                "		dataSource: {{\r\n" +
                "			transport: {{\r\n" +
                "				read: {{\r\n" +
                "					url: '{1}',\r\n" +
                "					dataType: 'json',\r\n" +
                "					type: 'POST',\r\n" +
                "					data: function (){{\r\n" +
                "						" + dataString + "\r\n" +
                "					}}\r\n" +
                "				}}\r\n" +
                "			}},\r\n" +
                "		schema: {{\r\n" +
                "			data: 'data', \r\n" +
                "			total: 'total', \r\n" +
                "			model: {{\r\n" +
                "				fields: {{\r\n"
                , gridId, urlParent);

            foreach (var column in columnsParent)
            {
                string columnType = string.Empty;

                if (column.Type == KendoGridColumnTypes.Number)
                {
                    columnType = "number";
                }
                else if (column.Type == KendoGridColumnTypes.String)
                {
                    columnType = "string";
                }

                result += "					" + column.Name + ": {type: '" + columnType + "'},\r\n";
            }

            result += string.Format(
                "				}}\r\n" +
                "			}}\r\n" +
                "		}},\r\n" +
                "		serverFiltering: " + (serverFiltering ? "true" : "false") + ",\r\n" +
                "		serverPaging: true, \r\n" +
                "		serverSorting: true, \r\n" +
                "		}},\r\n" +//
                "		pageable: {{\r\n" +
                "			pageSize: {0},\r\n" +
                "			pageSizes: [{0}, {0}*2, {0}*4]\r\n" +
                "		}},\r\n" +
                "		sortable: " + (sortable ? "true" : "false") + ",\r\n" +
                "		detailInit: detailInit,\r\n" +
                "		resizable: " + (resizable ? "true" : "false") + ",\r\n" +
                "		columns: [\r\n", pageSize);

            for (int i = 0; i < columnsParent.Length; i++)
            {
                //string dateFormat = string.Empty;
                string hiddenColumn = string.Empty;
                string FrozenColumn = string.Empty;

                //if (column.Type == "date")
                //	dateFormat = "format: '{0:MM/dd/yyyy}',\r\n";

                if (columnsParent[i].Hidden == true)
                {
                    hiddenColumn = "			hidden : true,\r\n";
                }
                if (columnsParent[i].Frozen == true)
                {
                    FrozenColumn = "			locked : true,\r\n";
                }

                result +=
                    "		{\r\n" +
                    "			field:'" + columnsParent[i].Name + "',\r\n" +
                    "			title:'" + columnsParent[i].Title + "',\r\n" +
                    //dateFormat +
                    hiddenColumn +
                    FrozenColumn +
                    "			width: " + columnsParent[i].Width + "\r\n" +
                    "		}";

                if (!(columnsParent.Length == (i + 1) && commandsParent == null && serverFiltering == false))
                {
                    result += ",\r\n";
                }
                else
                {
                    result += "\r\n";
                }
            }

            if (commandsParent != null)
            {
                for (int i = 0; i < commandsParent.Length; i++)
                {
                    string FrozenColumn = string.Empty;
                    if (commandsParent[i].Frozen == true)
                    {
                        FrozenColumn = "			locked : true,\r\n";
                    }

                    result +=
                        "		{\r\n" +
                        "			template: '<a href=\"" + commandsParent[i].Url + "\">" + commandsParent[i].Text + "</a>',\r\n" +
                        FrozenColumn +
                        "			width: 70,\r\n" +
                        "		}";

                    if (commandsParent.Length != (i + 1))
                    {
                        result += ",\r\n";
                    }
                    else
                    {
                        result += "\r\n";
                    }
                }
            }

            result += "		]";

            if (serverFiltering == true)
            {
                result +=
                    ",\r\n" +
                    "		filterable: {\r\n" +
                    "			mode: 'menu',\r\n" +
                    "			extra: false,\r\n" +
                    "			operators: {\r\n" +
                    "				string: {\r\n" +
                    "					contains: kendo.ui.FilterMenu.prototype.options.operators.string.contains,\r\n" +
                    "					doesnotcontain: kendo.ui.FilterMenu.prototype.options.operators.string.doesnotcontain,\r\n" +
                    "				}\r\n" +
                    "			}\r\n" +
                    "		}\r\n";
            }

            result +=
                "	});\r\n";

            result += string.Format(
                 " function detailInit(e) {{\r\n" +
                 "	var value = e.data." + filterColumnId + ";\r\n" +
                 "   $(\"<div/>\").appendTo(e.detailCell).kendoGrid({{\r\n" +
                 "		height: 200,\r\n" +
                 "		dataSource: {{\r\n" +
                 "			transport: {{\r\n" +
                 "				read: {{\r\n" +
                 "					url: '{0}',\r\n" +
                 "					dataType: 'json',\r\n" +
                 "					type: 'POST'\r\n" +
                 "				}}\r\n" +
                 "			}},\r\n" +
                 "		serverFiltering: " + (serverFiltering ? "true" : "false") + ",\r\n" +
                 "		serverPaging: true, \r\n" +
                 "		serverSorting: true, \r\n" +
                 "	    filter: {{ field: \"" + filterColumnId + "\", operator: \"eq\", value: value }},\r\n" +
                 "		schema: {{\r\n" +
                 "			data: 'data', \r\n" +
                 "			total: 'total', \r\n" +
                 "			model: {{\r\n" +
                 "				fields: {{\r\n"
                 , urlChild);

            foreach (var column in columnsChild)
            {
                string columnType = string.Empty;

                if (column.Type == KendoGridColumnTypes.Number)
                {
                    columnType = "number";
                }
                else if (column.Type == KendoGridColumnTypes.String)
                {
                    columnType = "string";
                }

                result += "					" + column.Name + ": {type: '" + columnType + "'},\r\n";
            }

            result += string.Format(
                "				}}\r\n" +
                "			}}\r\n" +
                "		}},\r\n" +

                "		}},\r\n" +//
                "		sortable: " + (sortable ? "true" : "false") + ",\r\n" +
                "		resizable: " + (resizable ? "true" : "false") + ",\r\n" +

                "		columns: [\r\n", pageSize);

            for (int i = 0; i < columnsChild.Length; i++)
            {
                //string dateFormat = string.Empty;
                string hiddenColumn = string.Empty;
                string FrozenColumn = string.Empty;

                //if (column.Type == "date")
                //	dateFormat = "format: '{0:MM/dd/yyyy}',\r\n";

                if (columnsChild[i].Hidden == true)
                {
                    hiddenColumn = "			hidden : true,\r\n";
                }
                if (columnsChild[i].Frozen == true)
                {
                    FrozenColumn = "			locked : true,\r\n";
                }

                result +=
                    "		{\r\n" +
                    "			field:'" + columnsChild[i].Name + "',\r\n" +
                    "			title:'" + columnsChild[i].Title + "',\r\n" +
                    //dateFormat +
                    hiddenColumn +
                    FrozenColumn +
                    "			width: " + columnsChild[i].Width + "\r\n" +
                    "		}";

                if (!(columnsChild.Length == (i + 1) && commandsChild == null && serverFiltering == false))
                {
                    result += ",\r\n";
                }
                else
                {
                    result += "\r\n";
                }
            }

            if (commandsChild != null)
            {
                for (int i = 0; i < commandsChild.Length; i++)
                {
                    string FrozenColumn = string.Empty;
                    if (commandsChild[i].Frozen == true)
                    {
                        FrozenColumn = "			locked : true,\r\n";
                    }

                    result +=
                        "		{\r\n" +
                        "			template: '<a href=\"" + commandsChild[i].Url + "\">" + commandsChild[i].Text + "</a>',\r\n" +
                        FrozenColumn +
                        "			width: 70,\r\n" +
                        "		}";

                    if (commandsChild.Length != (i + 1))
                    {
                        result += ",\r\n";
                    }
                    else
                    {
                        result += "\r\n";
                    }
                }
            }

            result += "		]";

            if (serverFiltering == true)
            {
                result +=
                    ",\r\n" +
                    "		filterable: {\r\n" +
                    "			mode: 'menu',\r\n" +
                    "			extra: false,\r\n" +
                    "			operators: {\r\n" +
                    "				string: {\r\n" +
                    "					contains: kendo.ui.FilterMenu.prototype.options.operators.string.contains,\r\n" +
                    "					doesnotcontain: kendo.ui.FilterMenu.prototype.options.operators.string.doesnotcontain,\r\n" +
                    "				}\r\n" +
                    "			}\r\n" +
                    "		}\r\n";
            }

            result +=
                "	});}\r\n" +
                "</script>";

            return new MvcHtmlString(result);
        }
    }
}
