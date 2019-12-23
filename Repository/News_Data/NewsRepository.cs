using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;
using System.Linq;
using Models.ViewModels;
using DataLibrary.Context;
using Models.News;
using Models.Classes;

namespace Repository.News_Data
{
    public interface IVideoRepository
    {
        List<NewsDataViewModels> NewsList { get; }
        bool AddVideo(NewsDataViewModels video, string videoPath);
        bool DeleteVideo(int videoId);
        NewsDataPaginationViewModels GetNewsList(int user_id, NewsTypes type, int page = 1, int pageSize = 4,
            string columnOrder = null, string sortByOrder = null);
        NewsDataPagination_a_ViewModels GetNewsList_a(int user_id, NewsTypes type, int page = 1, int pageSize = 4,
            string columnOrder = null, string sortByOrder = null);
    }

    public class VideoRepository : IVideoRepository
    {
        int PageSize = 4;
        AppIdentityDbContext context = null;

        public VideoRepository(AppIdentityDbContext cx)
        {
            context = cx;
        }

        public List<NewsDataViewModels> NewsList
        {
            get
            {
                return context.NewsData.Select(t => new NewsDataViewModels
                {
                    church = t.church,
                    country = t.country,
                    description = t.description,
                    fName = t.fName,
                    lName = t.lName,
                    person = t.person,
                    province = t.province,
                    status = t.status,
                    User_Id = t.user_id
                }).ToList();
            }
        }

        public bool AddVideo(NewsDataViewModels video, string videoPath)
        {
            bool RetVal = false;

            if (video == null)
            { return RetVal; }

            NewsData newsData = new NewsData
            {
                church = video.church,
                country = video.country,
                description = video.description,
                fName = video.fName,
                lName = video.lName,
                person = video.person,
                province = video.province,
                status = video.status,
                user_id = video.User_Id,
                Path = videoPath
            };

            try
            {
                context.Entry(newsData).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                context.SaveChanges();
                RetVal = true;
            }
            catch (Exception ex)
            {
                int i = 9;
            }

            return RetVal;
        }

        public bool DeleteVideo(int videoId)
        {
            bool RetVal = false;

            if (videoId == 0)
            { return RetVal; }

            NewsData newsData = context.NewsData.Where(t => t.Id == videoId).FirstOrDefault();

            try
            {
                context.Entry(newsData).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
                RetVal = true;
            }
            catch (Exception ex)
            {
                int i = 9;
            }

            return RetVal;
        }   

        public NewsDataPaginationViewModels GetNewsList(int user_id, NewsTypes type = NewsTypes.All,
                        int page = 1, int pageSize = 4, string columnOrder = null, string sortByOrder = null)
        {
            List<NewsDataViewModels> List = new List<NewsDataViewModels>();
            List<string> colList = new List<string> { "description", "path", "lname" };
            string colName = colList.Where(t => t == columnOrder?.Trim().ToLower()).FirstOrDefault();

            List<NewsData> list = null;
            if (columnOrder == null || colName == null || sortByOrder == null)
            {
                list = context.NewsData.Where(p => p.user_id == user_id)
            .OrderBy(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
            }
            else
            {
                if (colName == "description")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
                else if (colName == "path")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.Path).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.Path).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
                else if (colName == "lname")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.lName).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.lName).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
            }

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    NewsDataViewModels newsData = new NewsDataViewModels
                    {
                        church = item.church,
                        country = item.country,
                        description = item.description,
                        fName = item.fName,
                        lName = item.lName,
                        person = item.person,
                        province = item.province,
                        status = item.status,
                        User_Id = item.user_id,
                        Path = item.Path
                    };

                    List.Add(newsData);
                }

                PagingInfo pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = context.NewsData.Where(t => t.user_id == user_id).Count()
                };

                return new NewsDataPaginationViewModels { NewsDatasList = List, PagingInfo = pagingInfo };
            }
            else
            {
                return new NewsDataPaginationViewModels
                {
                    NewsDatasList = new List<NewsDataViewModels>(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = 0
                    }
                };
            }
        }

        public NewsDataPagination_a_ViewModels GetNewsList_a(int user_id, NewsTypes type = NewsTypes.All,
                int page = 1, int pageSize = 4, string columnOrder = "", string sortByOrder = "asc")
        {

            sortByOrder = sortByOrder ?? "asc";
            List<NewsDataViewModels> List = new List<NewsDataViewModels>();
            List<string> colList = new List<string> { "description", "path", "lname" };
            string colName = colList.Where(t => t == columnOrder?.Trim().ToLower()).FirstOrDefault();

            List<NewsData> list = null;
            if (string.IsNullOrEmpty(columnOrder) || colName == null || string.IsNullOrEmpty(sortByOrder))
            {
                list = context.NewsData.Where(p => p.user_id == user_id)
            .OrderBy(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
            }
            else
            {
                if (colName == "description")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.description).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
                else if (colName == "path")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.Path).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.Path).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
                else if (colName == "lname")
                {
                    if (sortByOrder == "asc")
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                         .OrderBy(t => t.lName).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                    else
                    {
                        list = context.NewsData.Where(p => p.user_id == user_id)
                            .OrderByDescending(t => t.lName).Skip((page - 1) * pageSize).Take<NewsData>(pageSize).ToList();
                    }
                }
            }

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    NewsDataViewModels newsData = new NewsDataViewModels
                    {
                        church = item.church,
                        country = item.country,
                        description = item.description,
                        fName = item.fName,
                        lName = item.lName,
                        person = item.person,
                        province = item.province,
                        status = item.status,
                        User_Id = item.user_id,
                        Path = item.Path
                    };

                    List.Add(newsData);
                }

                int TotalItems = context.NewsData.Where(t => t.user_id == user_id).Count();
                Paging_a_Info paging_A_Info_ = PopulatePagingInfo(TotalItems, page, pageSize);

                //PagingInfo pagingInfo = new PagingInfo
                //{
                //    CurrentPage = page,
                //    ItemsPerPage = PageSize,
                //    TotalItems = context.NewsData.Where(t => t.user_id == user_id).Count()
                //};

                return new NewsDataPagination_a_ViewModels { NewsDatasList = List, paging_A_Info = paging_A_Info_ ,
                                                            sortByColumn = columnOrder ,sortByOrder= sortByOrder };
            }
            else
            {
                return new NewsDataPagination_a_ViewModels
                {
                    NewsDatasList = new List<NewsDataViewModels>(),
                    paging_A_Info = new Paging_a_Info
                    {
                        //CurrentPage = page,
                        //ItemsPerPage = PageSize,
                        //TotalItems = 0
                    }
                };
            }
        }

        #region Private Methods
        private Paging_a_Info PopulatePagingInfo(int totalItems, int currentPage, int pageSize)
        {
            int startPage;
            int endPage;
            Paging_a_Info paging_A_Info = new Paging_a_Info();
            // calculate total pages
            int totalPages = (int)Math.Ceiling((decimal)(totalItems / pageSize));

            if (totalPages <= 10)
            {
                // less than 10 total pages so show all
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // more than 10 total pages so calculate start and end pages
                if (currentPage <= 6)
                {
                    startPage = 1;
                    endPage = 10;
                }
                else if (currentPage + 4 >= totalPages)
                {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                }
                else
                {
                    startPage = currentPage - 5;
                    endPage = currentPage + 4;
                }
            }

            paging_A_Info.startPage = startPage;
            paging_A_Info.endPage = endPage;
            paging_A_Info.totalPages = totalPages;
            paging_A_Info.currentPage = currentPage;



            return paging_A_Info;
        } 
        #endregion
    }
}
