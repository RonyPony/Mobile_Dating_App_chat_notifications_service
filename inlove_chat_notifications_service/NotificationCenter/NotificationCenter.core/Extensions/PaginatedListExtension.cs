using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotificationCenter.Core.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="PaginatedList{T}"/> class 
    /// </summary>
    public static class PaginatedListExtension
    {
        /// <summary>
        /// Extension to convert a <see cref="PaginatedList{T}"/> from one type to another
        /// </summary>
        /// <typeparam name="TSource">The type of the source list</typeparam>
        /// <typeparam name="TResult">The type of the resulting list</typeparam>
        /// <param name="source">The list to cast</param>
        /// <param name="selector">A function that will convert the items in the source list.</param>
        /// <returns>A new <see cref="PaginatedList{T}"/> of the resulting type.</returns>
        public static PaginatedList<TResult> Select<TSource, TResult>(this PaginatedList<TSource> source, Func<TSource, TResult> selector)
        {
            List<TResult> mappedItems = source.Items.Select(selector).ToList();

            return new PaginatedList<TResult>(mappedItems, source.TotalItems, source.PageIndex, source.PageSize);
        }
    }

}
