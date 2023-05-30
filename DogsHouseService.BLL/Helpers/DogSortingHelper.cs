using DogsHouseService.DAL.Entities;
using System.Linq.Expressions;

namespace DogsHouseService.BLL.Helpers
{
    public static class DogSortingHelper
    {
        public static IQueryable<Dog> ApplySortByAttribute(IQueryable<Dog> query, string attribute, string order)
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

        private static IQueryable<T> SortBy<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, string order)
        {
            return order == "asc" ? query.OrderBy(keySelector) : order == "desc" ? query.OrderByDescending(keySelector) : throw new ArgumentException("Invalid order value.");
        }
    }
}
