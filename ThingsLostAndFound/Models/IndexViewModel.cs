using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Models
{
    public class IndexViewModel
    {
        //Several List depends of the view executed
        public IEnumerable<Models.FoundObject> FoundObjectList { get; set; }
        public IEnumerable<Models.LostObject> LostObjectList { get; set; }
        public IEnumerable<Models.Message> MessagesList { get; set; }
        public List<int> IdNewmsgs { get; set; }
        public Pager Pager { get; set; }
    }

    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize)
        {
            // calculate total, start and end pages, pageSize is the number of objects per page
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}