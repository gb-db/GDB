using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.Classes;
using Models.Users;
using Models.ViewModels;
using Users.Models;

namespace Users.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "news-list")]
    public class ULListTagHelper : TagHelper
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public ULListTagHelper(UserManager<AppUser> usermgr,
                                  RoleManager<IdentityRole> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public SecurePageModel info { get;set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string str = @"<li id='post-885830' class='news-custom-posts-item news-custom-posts-small clearfix  type-post status-publish format-standard has-post-thumbnail '>" +
                        @"<figure class='news-custom-posts-thumb'>" +
                        @"<a class='news-thumb-icon news-thumb-icon-small' href='https://armlur.am/885830/' title='Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք'>" +
                        @"<img width='80' height='60' src='https://armlur.am/wp-content/uploads/2019/03/94c89b2cc1b04b4f37da2692a7027034-1-80x60.jpg' class='attachment-thumbnail_small size-thumbnail_small wp-post-image' alt=''  />" +
                        @"</a>" +
                        @"</figure>" +
                        @"<div class='news-custom-posts-header'>" +
                        @"<div class='news-custom-posts-small-title'>" +
                        @"<a href='https://armlur.am/885830/' title='Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք'> Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք </a>" +
                        @"</div>" +
                        @"<div class='news-meta entry-meta'>" +
                        @"<span class='entry-meta-date updated'>" +
                        @"<i class='fa fa-clock-o'></i>14.03 20:24" +
                        @"</span>" +
                        @"</div>" +
                        @"</div>" +
                        @"</li>";

            string strSummary = "";
            for (int i = 0; i < 15; i++)
            {
                strSummary += str;
            }

            output.Content.SetHtmlContent(strSummary);
        }
    }

    [HtmlTargetElement("ul", Attributes = "news-img-list")]
    public class ULIMGListTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string str = @"<li id='post-885830' class='news-custom-posts-item news-custom-posts-small clearfix  type-post status-publish format-standard has-post-thumbnail '>" +
                        @"<figure class='news-custom-posts-thumb'>" +
                        @"<a class='news-thumb-icon news-thumb-icon-small' href='https://armlur.am/885830/' title='Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք'>" +
                        @"<img width='180px' height='60px' src='/Images/l1.png' class='attachment-thumbnail_small size-thumbnail_small wp-post-image' alt=''  />" +
                        @"</a>" +
                        @"</figure>" +
                        @"<div class='news-custom-posts-header'>" +
                        @"<div class='news-custom-posts-small-title'>" +
                        @"<a href='https://armlur.am/885830/' title='Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք'> Հայաստանյան երեք բանկերի ֆինանսավորմամբ իրականացվել է հայկական ընկերության բաժնետոմսերի ձեռքբերման նշանակալի գործարք </a>" +
                        @"</div>" +
                        @"<div class='news-meta entry-meta'>" +
                        @"<span class='entry-meta-date updated'>" +
                        @"<i class='fa fa-clock-o'></i>14.03 20:24" +
                        @"</span>" +
                        @"</div>" +
                        @"</div>" +
                        @"</li>";

            string strSummary = "";
            for (int i = 0; i < 15; i++)
            {
                strSummary += str;
            }

            output.Content.SetHtmlContent(strSummary);
        }
    }

    [HtmlTargetElement("div", Attributes = "img-column2-list")]
    public class Caolumn2_ListTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {//image size is: 300x169
            string str = @"<div class='itemContainer' style='width:50.0%;'>"+
                         @"<div class='catItemView groupLinks'>" +
                         @"<h3 class='catItemTitle'>"+
                         @"<a href = ' / tv-series/fulhaus/item/111338-full-house-9-episode-1' >" +
                         @"Full House 9 - Episode 1" +
                         @"</a>" +
                         @"<span> <sup> ANONS</sup> </span>" +
                         @"</h3>" +
                         @"<div class='catItemImageBlock'>" +
                         @"<span class='catItemImage'>" +
                         @"<a href = ' / tv-series/fulhaus/item/111338-full-house-9-episode-1' title='Full House 9 - Episode 1'>" +
                         @"<img src = '/Images/girl.jpg' alt='Full House 9 - Episode 1' style='width:300px; height:100%;object-fit: contain ' />" +
                         @"</a>" +
                         @"</span>" +
                         @"<div class='clr'></div>" +
                         @"</div>" +
                         @"</div>" +
                         @"</div>"+
                         @"<div class='itemContainer itemContainerLast' style='width:50.0%;'>"+
                         @"<div class='catItemView groupLinks'>"+
                         @"<h3 class='catItemTitle'>"+
                         @"<a href = '/tv-series/fulhaus/item/106171-full-house-8-episode-25' >"+
                         @"Full House 8 - Episode 25 (Final)"+
                         @"</a>"+
                         @"</h3>"+
                         @"<div class='catItemImageBlock'>"+
                         @"<span class='catItemImage'>"+
                         @"<a href = '/tv-series/fulhaus/item/106171-full-house-8-episode-25' title='Full House 8 - Episode 25 (Final)'>"+
                         @"<img src = '/Images/girl.jpg' alt='Full House 8 - Episode 25 (Final)' style='width:300px; height:auto;' />"+
                         @"</a>"+
                         @"</span>"+
                         @"<div class='clr'></div>"+
                         @"</div>"+
                         @"</div>"+
                         @"</div>"+
                         @"<div class='clr'></div>";

            string strSummary = "";
            for (int i = 0; i < 13; i++)
            {
                strSummary += str;
            }

            output.Content.SetHtmlContent(strSummary);
        }
    }


    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context,
                TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                PageUrlValues["productPage"] = i;
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                }
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }

    //public class PagingInfo
    //{
    //    public int TotalItems { get; set; }
    //    public int ItemsPerPage { get; set; }
    //    public int CurrentPage { get; set; }

    //    public int TotalPages =>
    //        (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    //}
}
