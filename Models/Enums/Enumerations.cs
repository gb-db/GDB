using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Enums
{
    //public enum Country
    //{
    //    [Display(Name ="Select...")]
    //    None=0,
    //    Armenia = 1,
    //    Russia = 2,
    //    Canada = 3,
    //    USA = 4
    //}

    //public enum Church
    //{
    //    [Display(Name = "Select...")]
    //    None = 0,
    //    Armenian1 = 1,
    //    Armenian2 = 2,
    //    Armenian3 = 3,
    //    Armenian4 = 4
    //}

    public enum NewsTypes
    {
        All,
        video,
        image
    }

    public enum ImageModificationsType
    {
        InDivCenter,
        WithGivenWidth
    }
}
