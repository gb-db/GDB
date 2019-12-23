using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Classes
{
    class Miscellenious
    {
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }

    public class Paging_a_Info
    {
        public int PAGESIZE { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
        public int startPage { get; set; }
        public int endPage { get; set; }

        public string ORDER_ASC { get; set; } = "asc";
        public string ORDER_DESC { get; set; } = "desc";
        public string sortByColumn { get; set; }
        public string sortByOrder { get; set; }
    }

    public class AppSettings
    {
        public int PHOTO_WIDTH { get; set; }
        public int PHOTO_HEIGHT { get; set; }
        public int IMAGE_WIDTH { get; set; }
        public int IMAGE_HEIGHT { get; set; }
        public string emptyPhoto { get; set; }
        public string userIdStr { get; set; }
    }
}
