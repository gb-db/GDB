using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Classes
{
    class NewsInformation
    {
    }

    public class NewsSearchDataResponse
    {
        public string errorMessage { get; set; }
        public int totalRow { get; set; }
        public List<NewsSearchData> news { get; set; }
    }

    public class NewsSearchData
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
