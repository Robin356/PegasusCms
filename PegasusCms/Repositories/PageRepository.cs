using Microsoft.Extensions.Caching.Memory;
using Pegasus;
using Pegasus.Core.Data;
using Pegasus.Core.Extensions;
using PegasusCms.Models.Page;
using System.Collections.Generic;
using System.Linq;

namespace PegasusCms.Repositories
{
    internal class PageRepository
    {
        private const string MainMenuCacheKeyFormat = "PageRepository::MainMenu::{0}";
        private const string SideMenuCacheKeyFormat = "PageRepository::SideMenu::{0}";

        private IContext _Context;
        private IMemoryCache _MemoryCache;
        private Node _HomeNode;
        private Node _ContextNode;

        public PageRepository(IContext context, IMemoryCache memoryCache, Node homeNode, Node contextNode)
        {
            _Context = context;
            _MemoryCache = memoryCache;
            _HomeNode = homeNode;
            _ContextNode = contextNode;
        }

        private bool IsActiveMenuItem(Node menuNode)
        {
            if (menuNode == null) return false;
            var node = _ContextNode;
            if (menuNode == node) return true;
            while (node != null && node != _HomeNode)
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

        private void BuildMenu(NodeClass.Website.Page page)
        {
            var mainMenuCacheKey = string.Format(MainMenuCacheKeyFormat, page.Id);
            var sideMenuCacheKey = string.Format(SideMenuCacheKeyFormat, page.Id);
            var mainMenu = _MemoryCache.Get<Menu>(mainMenuCacheKey);
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
                var sideMenu = new Menu()
                {
                    MenuItems = sideMenuItems
                };
                _MemoryCache.Set(sideMenuCacheKey, sideMenu);
            }
        }

        public Menu GetMainMenu(NodeClass.Website.Page page)
        {
            BuildMenu(page);
            var mainMenuCacheKey = string.Format(MainMenuCacheKeyFormat, page.Id);
            return _MemoryCache.Get<Menu>(mainMenuCacheKey);
        }

        public Menu GetSideMenu(NodeClass.Website.Page page)
        {
            BuildMenu(page);
            var sideMenuCacheKey = string.Format(SideMenuCacheKeyFormat, page.Id);
            return _MemoryCache.Get<Menu>(sideMenuCacheKey);
        }
    }
}
