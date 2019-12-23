using Models.Heritage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Models.Classes
{
    public class Heritage
    {
        public List<her> hers { get; set; }
        public string message { get; set; }
        public bool success { get; set; }

        public List<List<her>> listArr { get; set; }
    }


    public class her
    {
        public int intId { get; set; }
        public string id { get; set; }
        public int user_id { get; set; }
        public int heratigeUser_Id { get; set; }
        public int parent_level { get; set; }
        public int parent_number { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        public int last_child { get; set; }
        public string her_src { get; set; }
        public string her_fn { get; set; }
        public string her_ln { get; set; }
        public string her_birthDate { get; set; }
        public string her_endDate { get; set; }
        public DateTime? birthDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<img_desc> info_data { get; set; }
        public string herString { get; set; }
        public string pattern { get; set; }
        public int tt_width { get; set; }
        public int tt_height { get; set; }
        public string flagImgPath { get; set; }
        public string flagImgPathBP { get; set; }
        public string countryName { get; set; }
        public string countryNameBP { get; set; }
        public bool isClosed { get; set; }
        public bool isEditable { get; set; }
    }


    public class img_desc
    {
        public string text { get; set; }
        public string url { get; set; }
    }

    public class HerRepData
    {
        public string path { get; set; }
        public Image image { get; set; }
        public Size size { get; set; }
        public string message { get; set; } = "";
    }

    public class s_tr
    {
        public string str { get; set; }
        public string heritageIds { get; set; }
    }

    public class s_strings
    {
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
    }

    public class AllTextBoxIds
    {//countryId_provinceId_cityId_curchId_countryBPId_provinceBPId_cityBPId_userId_heratigeUserId
        public int countryId { get; set; }
        public int provinceId { get; set; }
        public int cityId { get; set; }
        public int churchId { get; set; }
        public int countryBPId { get; set; }
        public int provinceBPId { get; set; }
        public int cityBPId { get; set; }
        public int heratigeUser_Id { get; set; }
        public int user_id { get; set; }
        
    }

    public class RetSimple
    {
        public int Id { get; set; }
        public int retId { get; set; }
        public int number { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public string card_ids { get; set; }
    }

    public class RetHerUpdate
    {
        public int Id { get; set; }
        public int retId { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public string her_fn { get; set; }
        public string her_ln { get; set; }
        public string her_birthDate { get; set; }
        public string her_endDate { get; set; }
        public string heratigeUser_Id { get; set; }
    }

    public class ForNewRecords
    {
        public string name { get; set; }
        public string objName { get; set; }
        public int countryId { get; set; }
        public int provinceId { get; set; }
        public int cityId { get; set; }
        public int churchId { get; set; }
    }

    public class RetHerClasses
    {
        public HeratigeUser heratigeUser { get; set; }
        public HeratigeData HeratigeData { get; set; }
    }

    public class DeleteData
    {
        public int user_id { get; set; }
        public int parent_level { get; set; }
        public int parent_number { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        public int heratigeUser_Id { get; set; }
        public string pattern { get; set; }
    }

    public class CardIds
    {
        public int user_Id { get; set; }
        public int heratigeUser_Id { get; set; }
        public int parentLevel { get; set; }
        public int parentNumber { get; set; }
        public int level { get; set; }
        public int number { get; set; }
    }

    public class InfoObject
    {
        public string message { get; set; }
        public List<InfoItem> info_items { get; set; }
    }

    public class InfoItem
    {
        public int id { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string path { get; set; }
        public int order_number { get; set; }
        public string card_ids { get; set; }
    }
};
