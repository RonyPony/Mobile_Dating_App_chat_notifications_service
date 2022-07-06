using System;
using System.Collections.Generic;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a list separated by pages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T>
    {
        /// <summary>
        /// The number of this page
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// The number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The items in this list
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// The number of items among all pages
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PaginatedList() { }

        /// <summary>
        /// The constructor of a <see cref="PaginatedList{T}"/>
        /// </summary>
        /// <param name="items">The items of this list</param>
        /// <param name="count">The number of items of all pages</param>
        /// <param name="pageIndex">The index of this page</param>
        /// <param name="pageSize">The number of items per page</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
            TotalItems = count;
        }
    }
}
