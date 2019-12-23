using Models.Classes;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.ViewModels
{
    public class NewsDataViewModels
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int User_Id { get; set; }

        public bool status { get; set; }

        [Display(Name = "Country")]
        public int country { get; set; }

        public int province { get; set; }

        [Display(Name = "Church")]
        public int church { get; set; }

        public string person { get; set; }

        public string fName { get; set; }

        [Display(Name = "Name")]
        public string lName { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        public string Path { get; set; }

        public PagingInfo pagingInfo { get; set; }

        public string message { get; set; }
    }

    public class NewsDataPaginationViewModels
    {
        public List<NewsDataViewModels> NewsDatasList { get; set; }

        public int PAGESIZE { get; set; }
        public int currentPage { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }

    public class NewsDataPagination_a_ViewModels
    {
        public List<NewsDataViewModels> NewsDatasList { get; set; }

        public int PAGESIZE { get; set; }
        public int currentPage { get; set; }
        public string sortByOrder { get; set; } = "";
        public string sortByColumn { get; set; } = "";

        public Paging_a_Info paging_A_Info { get; set; }
    }

    public class SimpleViewModel
    {
        public string message { get; set; }
        public string heritageIds { get; set; }
    }

    public class UploadPhotoViewModel
    {
        public string message { get; set; }
        public string heritageIds { get; set; }
        public string path { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string infoItemsStr { get; set; }
        public string rootHerImg { get; set; }
        public string rootHerName { get; set; }
        public List<InfoItem> infoItems { get; set; }
    }

    public class HerItem
    {
        public int heratigeUser_Id { get; set; }
        public int user_id { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string countryBP { get; set; }
        public string provinceBP { get; set; }
        public string cityBP { get; set; }
        public string LN_FN { get; set; }
        //userId_heratigeUser_Id_parentLevel_parentNumber_level_number
        public string heritageIds { get; set; }
        public bool isOpend { get; set; }
        public bool isEditable { get; set; }
        public string flagImgPath { get; set; }
        public string flagImgPathBP { get; set; }
        public string message { get; set; }
    }

    public class ItemInfoViewModel
    {
        public List<HerItem> herItems { get; set; }
        public string message { get; set; }
    }
}
