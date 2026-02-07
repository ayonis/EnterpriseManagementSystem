using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common
{
  public class PagedList<T>
   {
            public int PageNumber { get; private set; }
            public int PageSize { get; private set; } = 20;
            public int TotalCount { get; private set; }
            public int TotalPages { get; private set; } = 0;
            public List<T> Items { get; private set; } = new List<T>();

            public bool HasPrevious => PageNumber > 1;
            public bool HasNext => PageNumber < TotalPages;

            public PagedList(int totalCount, int pageNumber, int pageSize, List<T> items)
            {
                TotalCount = totalCount;
                PageSize = pageSize;
                PageNumber = pageNumber;
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                Items = items;
            }

            public static async Task<PagedList<T>> CreateAsync(int totalCount, int pageNumber, int pageSize, List<T> items)
            {
                return new PagedList<T>(totalCount, pageNumber, pageSize, items);
            }
        }

    
}
