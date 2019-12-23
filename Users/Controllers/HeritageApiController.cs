using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.Classes;
using Repository.Heritage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Users.Controllers
{
    [Route("Api/[Controller]")]
    public class HeritageApiController : Controller
    {
        public IHeritageRepository heritageRepository;
        private IHostingEnvironment _environment;
        private IConfiguration _config;
        //public string emptyPhoto = "/Images/Heritage/empty.png";// Temporary
        public List<string> list = new List<string> {
            @"ՀՀ պաշտպանության նախարար Դավիթ Տոնոյանը սահմանային իրավիճակի վերաբերյալ անհանգստանալու խնդիր չի տեսնում:",
            @"Այս մասին Տոնոյանն ասաց հուլիսի 22-ին լրագրողների հետ զրույցում:",
            @"«Որոշ մասերում վերահսկելի լարվածություն է առկա, բայց անհանգստանալու խնդիր չունենք»,- ասաց Տոնոյանը:"
        };

        public HeritageApiController(IHeritageRepository hereRep, IHostingEnvironment env, IConfiguration config)
        {
            heritageRepository = hereRep;
            _environment = env;
            _config = config;
        }

        [Route("[Action]")]
        [HttpPost]
        public IActionResult RemovePhoto([FromBody]Heritage heritage)
        {
            AppSettings appSettings = heritageRepository.GetAppSettings(_config);
            string folderPath = _environment.WebRootPath;
            RetSimple retVal = new RetSimple { success = false,message="There was problem to remove photo." };
            //string card_id = "";
            if (heritage?.hers != null && heritage?.hers.Count > 0)
            {
                retVal = heritageRepository.RemovePhoto((heritage.hers[0]).id, folderPath, appSettings.emptyPhoto);
            }

            return new JsonResult(retVal);
        }



        #region Edit data   
        [Route("[Action]")]
        [HttpPost]
        public IActionResult PostHerData([FromBody]s_tr textarea)
        {
            List<string> llist = (textarea.str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            if (llist == null || llist.Count == 0)
            {
                return new JsonResult(new { success = false, message = "Enter comment before saving." });
            }

            RetSimple retSimple = heritageRepository.SaveCommentToDB(textarea);

            string sendStr = "";
            List<string> list_ = new List<string>();
            list_.AddRange(llist);
            //list_.ForEach(t => t  =@"<p>" + t + @"</p>");
            foreach (var item in list_)
            {
                sendStr += @"<p>" + item + @"</p>";
            }

            return new JsonResult(new { success = true, sendStr = sendStr, order_number = retSimple.Id, card_ids = retSimple.card_ids });
        }

        [Route("[Action]")]
        public IActionResult Autocomplate(string term, string className,string allTextBoxIds)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                List<LabelValue> list = heritageRepository.GetAutocomplateList(term.Trim(), className, allTextBoxIds);


                if (list != null && list.Count > 0)
                {

                    return Json(list);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [Route("[Action]")]
        public IActionResult AutocomplateMain(string term, string className, string allTextBoxIds)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                List<ItemInfo> list = heritageRepository.GetAutocomplateListMain(term.Trim(), className, allTextBoxIds);
                return Json(list);
            }
            else
            {
                return Json(new List<ItemInfo>());
            }
        }

        [Route("[Action]")]
        [HttpPost]
        public IActionResult InsertNewRecord([FromBody]ForNewRecords forNewRecords)
        {
            int id_ = 0;
            bool succ = true;
            string message_ = "";
            switch (forNewRecords.objName)
            {
                case "country":
                    id_ = heritageRepository.InsertNewCountry(forNewRecords.name);
                    break;
                case "province":
                    id_ = heritageRepository.InsertNewProvince(forNewRecords.name, forNewRecords.countryId);
                    break;
                case "city":
                    id_ = heritageRepository.InsertNewCity(forNewRecords.name, forNewRecords.countryId, forNewRecords.provinceId);
                    break;
                case "church":
                    id_ = heritageRepository.InsertNewChurch(forNewRecords.name, forNewRecords.countryId, forNewRecords.provinceId, forNewRecords.cityId);
                    break;
                default:
                    break;
            }

            if (id_ == 0)
            {
                succ = false;
                message_ = "There was problem to entere new record!";
            }


            return new JsonResult(new { success = succ, obj = forNewRecords.objName, message = message_ });
        }

        [Route("[Action]")]
        public IActionResult GetHreitageById(int user_id, int herUserId)
        {
            Heritage heritage_ = heritageRepository.GetHeritageById(user_id, herUserId);
            return Json(new { success = heritage_.success, heritage = heritage_, message = heritage_.message });
        }

        [Route("[Action]")]
        [HttpPost]
        public IActionResult DeleteHeritageLineFrom([FromBody]DeleteData deleteData)
        {
            RetSimple retSimple = heritageRepository.DeleteHeritageLineFrom(deleteData);

            return new JsonResult(retSimple);
        }

        [Route("[Action]")]
        [HttpPost]
        public IActionResult DeleteImageComment([FromBody]s_tr deleteData)
        {
            string folderPath = _environment.WebRootPath;
            RetSimple retSimple = heritageRepository.DeleteImageComment(deleteData, folderPath);

            return new JsonResult(retSimple);
        }
        #endregion
    }
}
