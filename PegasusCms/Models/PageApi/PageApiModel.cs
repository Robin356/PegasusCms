
using PegasusCms.Models.Page;

namespace PegasusCms.Models.PageApi
{
    public class PageApiModel
    {
        public Menu MainMenu { get; set; }
        public Menu SideMenu { get; set; }
        public string PageTitle { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
