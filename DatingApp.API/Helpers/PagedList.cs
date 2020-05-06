using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            this.TotalCount= count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            // כמה משתמשים בסך הכל יש חלקי כמה נכנסים בדף אחד
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            // אם אני בעמוד השני וכל עמוד יהיה 5 משתמשים הוא יקח 1*5 כלומר מהמקום החמישי נדלג על 5 הראשונים וניקח את 5 הבאים
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize); 
        }
    }
}