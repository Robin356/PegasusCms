using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PegasusCms.Models.Page;
using Pegasus.Core.Data;
using Pegasus.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using Pegasus;

namespace PegasusCms.Controllers
{
    public class PageController : Controller
    {
        private IContext _Context;
        private IMemoryCache _MemoryCache;

        private const string MainMenuCacheKeyFormat = "MainMenu::{0}";
        private const string SideMenuCacheKeyFormat = "SideMenu::{0}";
        public PageController(IContext context, IMemoryCache memoryCache)
        {
            _Context = context;
            _MemoryCache = memoryCache;
        }

        private bool IsActiveMenuItem(Node menuNode)
        {
            if (menuNode == null) return false;
            var node = _Context.Node;
            if (menuNode == node) return true;
            while (node != null && node != _Context.Site.HomeNode)
            {
                if (menuNode == node) return true;
                node = node.Parent;
            }
            return false;
        }

        private MenuItem MenuItemFromPage(NodeClass.Website.Page page)
        {
            return new MenuItem()
            {
                Title = page.MenuTitle ?? page.PageTitleSingle,
                Url = _Context.Site.GetPageUrl(page.InnerNode),
                Active = IsActiveMenuItem(page.InnerNode)
            };
        }

        private void AddSubitemsToMenu(List<MenuItem> menuItems, NodeClass.Website.Page page)
        {
            foreach (var childPage in page.InnerNode.GetChildren().GetClass<NodeClass.Website.Page>())
            {
                if (childPage.ShowInMenu)
                {
                    var menuItem = MenuItemFromPage(childPage);
                    menuItems.Add(menuItem);
                    var childMenuItems = new List<MenuItem>();
                    AddSubitemsToMenu(childMenuItems, childPage);
                    if (childMenuItems.Any())
                    {
                        menuItem.SubMenuItems = childMenuItems;
                    }
                }
            }
        }

        private void BuildViewBag()
        {
            var page = _Context.Node.GetClass<NodeClass.Website.Page>();
            ViewBag.Title = page.PageTitle;
            var mainMenuCacheKey = string.Format(MainMenuCacheKeyFormat, page.Id);
            var sideMenuCacheKey = string.Format(SideMenuCacheKeyFormat, page.Id);
            var mainMenu = _MemoryCache.Get<Menu>(mainMenuCacheKey);
            var sideMenu = _MemoryCache.Get<Menu>(sideMenuCacheKey);
            if (mainMenu == null)
            {
                var home = _Context.Site.HomeNode.GetClass<NodeClass.Website.Page>();
                List<MenuItem> sideMenuItems = null;
                var menuItems = new List<MenuItem>();
                // Add Home node
                menuItems.Add(MenuItemFromPage(home));
                // First Level
                foreach (var level1Page in home.InnerNode.GetChildren().GetClass<NodeClass.Website.Page>())
                {
                    if (level1Page.ShowInMenu)
                    {
                        var menuItem = MenuItemFromPage(level1Page);
                        if (level1Page.InnerNode.ChildCount > 0)
                        {
                            var subMenuItems = new List<MenuItem>();
                            AddSubitemsToMenu(subMenuItems, level1Page);
                            menuItem.SubMenuItems = subMenuItems;
                            if (menuItem.Active)
                            {
                                // Active in sidebar
                                sideMenuItems = subMenuItems;
                            }
                        }
                        menuItems.Add(menuItem);
                    }
                }
                mainMenu = new Menu()
                {
                    MenuItems = menuItems
                };
                _MemoryCache.Set(mainMenuCacheKey, mainMenu);
                sideMenu = new Menu()
                {
                    MenuItems = sideMenuItems
                };
                _MemoryCache.Set(sideMenuCacheKey, sideMenu);
            }
            ViewBag.MainMenu = mainMenu;
            ViewBag.SideMenu = sideMenu;
        }

        public ActionResult Index()
        {
            BuildViewBag();
            return View(_Context.Node.GetClass<NodeClass.Website.ContentPage>());
        }

    }
}
