using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.ViewModels
{
    public class HeritageViewModel
    {
        public int user_id { get; set; }

        public int user_her_id { get; set; }

        [Display(Name ="First Name")]
        public string fName { get; set; }

        [Display(Name = "Last Name")]
        public string lName { get; set; }

        [Display(Name = "Mother's First Name")]
        public string fNameParentM { get; set; }

        [Display(Name = "Mother's Last Name")]
        public string lNameParentM { get; set; }

        [Display(Name = "Father's First Name")]
        public string fNameParentF { get; set; }

        [Display(Name = "Father's Last Name")]
        public string lNameParentF { get; set; }

        public DateTime? DOB { get; set; }
        public string DOB_str { get; set; }

        public DateTime? PWD { get; set; }
        public string PWD_str { get; set; }

        [Display(Name = "Country")]
        public string country { get; set; }
        public int countryId { get; set; }

        [Display(Name = "Province")]
        public string province { get; set; }
        public int provinceId { get; set; }

        [Display(Name = "City")]
        public string city { get; set; }
        public int cityId { get; set; }

        [Display(Name = "Church")]
        public string church { get; set; }
        public int churchId { get; set; }



        [Display(Name = "Country(Birth Place)")]
        public string countryBP { get; set; }
        public int countryBPId { get; set; }

        [Display(Name = "Province(Birth Place)")]
        public string provinceBP { get; set; }
        public int provinceBPId { get; set; }

        [Display(Name = "City(Birth Place)")]
        public string cityBP { get; set; }
        public int cityBPId { get; set; }




        [Display(Name = "Information")]
        public string information { get; set; }

        public string data_information { get; set; }

        public string message { get; set; }
    }
}
