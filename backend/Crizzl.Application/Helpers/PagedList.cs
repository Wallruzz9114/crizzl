using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Application.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items, int totalItems, int pageNumber, int itemsPerPage)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;

            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int itemsPerPage)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, itemsPerPage);
        }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }
}