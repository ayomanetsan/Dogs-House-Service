using DogsHouseService.DAL.Entities;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DogsHouseService.BLL.Helpers
{
    public static class DogSortingHelper
    {
        public static IQueryable<Dog> ApplySortByAttribute(IQueryable<Dog> query, string attribute, SortOrder order)
        {
            switch (attribute)
            {
                case "name":
                    return SortBy(query, d => d.Name, order);
                case "color":
                    return SortBy(query, d => d.Color, order);
                case "tail_length":
                    return SortBy(query, d => d.Tail_Length, order);
                case "weight":
                    return SortBy(query, d => d.Weight, order);
                default:
                    throw new ArgumentException("Invalid attribute value.");
            }
        }

        private static IQueryable<T> SortBy<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, SortOrder order)
        {
            return order == SortOrder.Ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
        }
    }
}
