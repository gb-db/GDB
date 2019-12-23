using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.ViewModels;
using Repository.News_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Components
{
    public class TableWithPagination : ViewComponent
    {
        private IVideoRepository videoRepository;

        public TableWithPagination(IVideoRepository videoRep)
        {
            videoRepository = videoRep;
        }

        public IViewComponentResult Invoke(Tuple<int, int> info)
        {
            if (info == null)
            {
                return View(videoRepository.GetNewsList(2, NewsTypes.All, 1));
            }
            else
            {
                return View(videoRepository.GetNewsList(info.Item2, NewsTypes.All, info.Item1));
            }
        }
    }
}
