using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pegasus;
using PegasusCms.Models.PageApi;
using PegasusCms.Repositories;
using System;

namespace PegasusCms.Controllers
{
    public class PageApiController : ControllerBase
    {
        private IContext _Context;
        private IMemoryCache _MemoryCache;

        public PageApiController(IContext context, IMemoryCache memoryCache)
        {
            _Context = context;
            _MemoryCache = memoryCache;
        }

        [HttpGet, Route("Api/Page")]
        public ActionResult<PageApiModel> Page(string path)
        {
            var homeNode = _Context.Database.GetNode(Constants.Nodes.Home_Id);
            var contextNode = _Context.Database.GetNodeByPath(homeNode.Path + path);
            var page = contextNode.GetClass<NodeClass.Website.ContentPage>();
            var pageRepository = new PageRepository(_Context, _MemoryCache, homeNode, contextNode);
            return new PageApiModel()
            {
                MainMenu = pageRepository.GetMainMenu(page),
                SideMenu = pageRepository.GetSideMenu(page),
                PageTitle = page.PageTitle,
                Title = page.ContentTitle,
                Content = page.ContentBody
            };
        }
    }
}
