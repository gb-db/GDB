using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Models.Enums;

namespace Models.News
{
    public class NewsData
    {
        public int Id { get; set; }

        [StringLength(8)]
        public string UserCode { get; set; }

        public int user_id { get; set; }

        [StringLength(1500)]
        public string Path { get; set; }

        public bool status { get; set; }

        public int country { get; set; }

        public int province { get; set; }

        public int church { get; set; }

        [StringLength(50)]
        public string person { get; set; }

        [StringLength(50)]
        public string fName { get; set; }

        [StringLength(50)]
        public string lName { get; set; }

        [StringLength(500)]
        public string description { get; set; }
    }
}
