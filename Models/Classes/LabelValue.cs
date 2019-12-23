using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Classes
{
    public class LabelValue
    {
        public string label { get; set; }

        public string value { get; set; }

        //public ItemInfo itemInfo { get; set; }
    }

    public class ItemInfo
    {
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
    }
}
