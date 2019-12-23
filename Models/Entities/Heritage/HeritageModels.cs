using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Heritage
{
    public class HeratigeUser
    {
        [Key()]
        public int HeratigeUserId { get; set; }

        [StringLength(8)]
        public string UserCode { get; set; }

        public int user_id { get; set; }

        [StringLength(50)]
        public string lName { get; set; }

        [StringLength(50)]
        public string fName { get; set; }

        public bool isActive { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public virtual ICollection<HeratigeData> HeratigeDatas { get; set; }

        public virtual ICollection<HerInfoSrc> HerInfoSrcs { get; set; }
    }

    public class HeratigeData
    {
        public int Id { get; set; }

        [StringLength(8)]
        public string UserCode { get; set; }

        public int user_id { get; set; }

        public int parent_level { get; set; }
        public int parent_number { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        public int last_child { get; set; }

        public int tt_width { get; set; }
        public int tt_height { get; set; }

        public DateTime? DOB { get; set; }//

        public DateTime? PWD { get; set; }//

        [StringLength(1500)]
        public string ImagePath { get; set; }

        [StringLength(1500)]
        public string ImageFolderPath { get; set; }

        public bool isActive { get; set; }

        public bool isEditable { get; set; }

        public bool isClosed { get; set; }

        public int CountryId { get; set; }
        public Country country { get; set; }

        public int? provinceId { get; set; }
        public Province province { get; set; }

        public int? cityId { get; set; }
        public City city { get; set; }//

        public int? churchId { get; set; }
        public Church church { get; set; }

        [ForeignKey("Country")]
        public int? CountryBPId { get; set; }
        public Country countryBP { get; set; }

        [ForeignKey("Country")]
        public int? provinceBPId { get; set; }
        public Province provinceBP { get; set; }

        [ForeignKey("Country")]
        public int? cityBPId { get; set; }
        public City cityBP { get; set; }//

        [StringLength(50)]
        public string person { get; set; }

        [StringLength(50)]
        public string fName { get; set; }

        [StringLength(50)]
        public string lName { get; set; }

        public string information { get; set; }

        [StringLength(50)]
        public string fNameParentF { get; set; }

        [StringLength(50)]
        public string lNameParentF { get; set; }

        [StringLength(50)]
        public string fNameParentM { get; set; }

        [StringLength(50)]
        public string lNameParentM { get; set; }

        [StringLength(5000)]
        public string pattern { get; set; }

        public DateTime dateTime { get; set; }

        [ForeignKey("HeratigeUser")]
        public int HeratigeUserId { get; set; }

        public HeratigeUser HeratigeUser { get; set; }
    }

    public class HeratigePermission
    {
        public int Id { get; set; }

        [StringLength(8)]
        public string UserCode { get; set; }

        public int user_id { get; set; }

        public bool isReadable { get; set; }
        public bool isWritable { get; set; }
        public bool isRoot { get; set; }
        public bool permit { get; set; }
        public DateTime dateTime { get; set; }

        [ForeignKey("HeratigeUser")]
        public int HeratigeUserId { get; set; }

        public HeratigeUser HeratigeUser { get; set; }
    }

    public class HerInfoSrc
    {
        public int HerInfoSrcId { get; set; }

        public int user_id { get; set; }
        public int parent_level { get; set; }
        public int parent_number { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        // public int HeratigeUserId { get; set; }

        public bool isActive { get; set; }
        public string desc { get; set; }

        [ForeignKey("HeratigeUser")]
        public int HeratigeUserId { get; set; }
        public HeratigeUser HeratigeUser { get; set; }

        public List<Info_Src> infoSrcs { get; set; }
    }

    public class Info_Src
    {
        public int Id { get; set; }
        public int number { get; set; }
        [StringLength(10000)]
        public string comment { get; set; }
        [StringLength(1500)]
        public string src { get; set; }
        public int tt_width { get; set; }
        public int tt_height { get; set; }
        public bool isActive { get; set; }

        [ForeignKey("HerInfoSrc")]
        public int? HerInfoSrcId { get; set; }
        public HerInfoSrc herInfoSrcs { get; set; }
    }

    public class Country
    {
        public int CountryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string ShortName { get; set; }

        [StringLength(150)]
        public string imgPath { get; set; }

        public bool isActive { get; set; }

        public ICollection<Province> Provinces { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Church> Churches { get; set; }
    }

    public class Province
    {
        public int provinceId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(10)]
        public string ShortName { get; set; }

        public bool isActive { get; set; }

        [ForeignKey("Country")]
        public int? countryId { get; set; }
        public Country country { get; set; }
    }

    public class City
    {
        public int cityId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(10)]
        public string ShortName { get; set; }

        public bool isActive { get; set; }

        [ForeignKey("Province")]
        public int? provinceId { get; set; }
        public Province province { get; set; }

        [ForeignKey("Country")]
        public int? countryId { get; set; }
        public Country country { get; set; }
    }

    public class Church
    {
        public int churchId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(10)]
        public string ShortName { get; set; }

        public bool isActive { get; set; }

        [ForeignKey("Country")]
        public int? countryId { get; set; }
        public Country country { get; set; }

        [ForeignKey("Province")]
        public int? provinceId { get; set; }
        public Province province { get; set; }

        [ForeignKey("City")]
        public int? cityId { get; set; }
        public City city { get; set; }
    }
}
