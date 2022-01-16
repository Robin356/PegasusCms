using System.Collections.Generic;

namespace PegasusCms.Models.PageApi
{
    public class Menu
    {
        public IEnumerable<MenuItem> MainMenu { get; set; }
        public IEnumerable<MenuItem> SideMenu { get; set; }
    }
}
