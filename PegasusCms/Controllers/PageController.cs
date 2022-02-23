using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pegasus;
using PegasusCms.Repositories;

namespace PegasusCms.Controllers
{
    public class PageController : Controller
    {
        private IContext _Context;
        private IMemoryCache _MemoryCache;

        public PageController(IContext context, IMemoryCache memoryCache)
        {
            _Context = context;
            _MemoryCache = memoryCache;
        }


        private void BuildViewBag()
        {
            var page = _Context.Node.GetClass<NodeClass.Website.Page>();
            var pageRepository = new PageRepository(_Context, _MemoryCache, _Context.Site.HomeNode, _Context.Node);
            ViewBag.Title = page.PageTitle;
            ViewBag.MainMenu = pageRepository.GetMainMenu(page);
            ViewBag.SideMenu = pageRepository.GetSideMenu(page);
        }

        public ActionResult Index()
        {
            BuildViewBag();
            return View(_Context.Node.GetClass<NodeClass.Website.ContentPage>());
        }

    }
}
