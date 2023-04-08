using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Hadi.Cms.Infrastructure.Utilities.KendoRequestParameters
{
    public class KendoRequestParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string FilterLogic { get; set; }
        public SortingInfo[] Sortings { get; set; }
        public FilterInfo[] Filters { get; set; }
        public static KendoRequestParameters Current
        {
            get
            {
                var result = new KendoRequestParameters();

                result.Populate();

                return result;
            }
        }
        public IQueryable ApplyToEntities(IQueryable dbSet)
        {
            Type elementType = dbSet.ElementType;
            IQueryable result = null;

            if (this.Filters?.Length > 0)
            {
                foreach (var filter in this.Filters)
                {
                    // * Use expression tree to build dynamic query to represent the following query:
                    // * elements.Where(i => i.filterField.Contains(filterValue))

                    ParameterExpression pe = Expression.Parameter(elementType, "i");

                    Expression predicateBody = null;

                    if (filter.Operator == FilterOperations.Contains)
                    {
                        predicateBody = Expression.Call(Expression.Property(pe, filter.Field), typeof(string).GetMethod("Contains"), Expression.Constant(filter.Value));
                    }
                    else if (filter.Operator == FilterOperations.NotContains)
                    {
                        Expression right = Expression.Call(Expression.Property(pe, filter.Field), typeof(string).GetMethod("Contains"), Expression.Constant(filter.Value));
                        predicateBody = Expression.Not(right);
                    }
                    else if (filter.Operator == FilterOperations.StartsWith)
                    {
                        MethodInfo startsWithMethod = typeof(string).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).First(m => m.Name == "StartsWith" && m.GetParameters().Count() == 1);
                        predicateBody = Expression.Call(Expression.Property(pe, filter.Field), startsWithMethod, Expression.Constant(filter.Value));
                    }

                    MethodCallExpression whereCallExpression = Expression.Call(
                        typeof(Queryable),
                        "Where",
                        new Type[] { elementType },
                        dbSet.AsQueryable().Expression,
                        Expression.Lambda(predicateBody, pe));

                    result = dbSet.AsQueryable().Provider.CreateQuery(whereCallExpression);
                }
            }
            else
            {
                result = dbSet.AsQueryable();
            }

            if (this.Sortings?.Length > 0)
            {
                foreach (var sorting in this.Sortings)
                {
                    // * Use expression tree to build dynamic query to represent the following query:
                    // * elements.OrderBy(i => i.sortingOn) or elements.OrderByDescending(i => i.sortingOn)

                    ParameterExpression pe = Expression.Parameter(elementType, "i");

                    Expression predicateBody = null;

                    predicateBody = Expression.Property(pe, sorting.SortOn);

                    string orderByMethod = null;

                    if (sorting.SortOrder == "asc")
                        orderByMethod = "OrderBy";

                    else if (sorting.SortOrder == "desc")
                        orderByMethod = "OrderByDescending";

                    Type sortPropertyType = elementType.GetProperty(sorting.SortOn).PropertyType;

                    MethodCallExpression OrderByCallExpression = Expression.Call(
                        typeof(Queryable),
                        orderByMethod,
                        new Type[] { elementType, sortPropertyType },
                        Expression.Call(typeof(Queryable), "Cast", new Type[] { elementType }, result.Expression),
                        Expression.Lambda(predicateBody, pe));

                    result = result.AsQueryable().Provider.CreateQuery(OrderByCallExpression);
                }
            }
            else
            {
                Type sortPropertyType = elementType.GetProperties()[0].PropertyType;

                ParameterExpression pe = Expression.Parameter(elementType, "i");

                Expression predicateBody = Expression.Property(pe, elementType.GetProperties()[0].Name);

                // * Use expression tree to build dynamic query to represent the following query:
                // * result.Cast<Models.Project>().OrderBy<Models.Project, int>()

                MethodCallExpression OrderByCallExpression = Expression.Call(
                    typeof(Queryable),
                    "OrderBy",
                    new Type[] { elementType, sortPropertyType },
                    Expression.Call(typeof(Queryable), "Cast", new Type[] { elementType }, result.Expression),
                    Expression.Lambda(predicateBody, pe));

                result = result.AsQueryable().Provider.CreateQuery(OrderByCallExpression);
            }

            return result;
        }
        internal void Populate()
        {
            if (HttpContext.Current != null)
            {
                HttpRequest request = HttpContext.Current.Request;

                // Default values
                int page = 0;
                int pageSize = 20;
                int skip = 0;
                int take = 20;
                string filterLogic = "AND";

                int.TryParse(request["page"], out page);
                int.TryParse(request["pageSize"], out pageSize);
                int.TryParse(request["skip"], out skip);
                int.TryParse(request["take"], out take);
                filterLogic = String.IsNullOrEmpty(request["filter[logic]"]) ? "AND" : request["filter[logic]"];

                //build sorting objects
                var sorts = new List<SortingInfo>();
                var x = 0;

                while (x < 20)
                {
                    var sortDirection = request["sort[" + x + "][dir]"];

                    if (sortDirection == null)
                    {
                        break;
                    }

                    var sortOn = request["sort[" + x + "][field]"];

                    if (sortOn != null)
                    {
                        sorts.Add(new SortingInfo { SortOn = sortOn, SortOrder = sortDirection });
                    }

                    x++;
                }

                Sortings = sorts.ToArray();

                //build filter objects
                var filters = new List<FilterInfo>();

                x = 0;

                while (x < 20)
                {
                    var field = request["filter[filters][" + x + "][field]"];
                    if (field == null)
                    {
                        break;
                    }

                    var val = request["filter[filters][" + x + "][value]"] ?? string.Empty;
                    var strop = request["filter[filters][" + x + "][operator]"];

                    if (strop != null)
                    {
                        filters.Add(new FilterInfo
                        {
                            Operator = FilterInfo.ParseOperator(strop),
                            Field = field,
                            Value = val
                        });
                    }

                    x++;
                }

                Filters = filters.ToArray();
            }
        }
    }
}