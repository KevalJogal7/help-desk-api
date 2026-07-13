using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortBy,
        bool descending = false)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var property = typeof(T).GetProperty(
            sortBy,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Instance);

        if (property == null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = descending
            ? "OrderByDescending"
            : "OrderBy";

        var result = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExpression));

        return query.Provider.CreateQuery<T>(result);
    }
}