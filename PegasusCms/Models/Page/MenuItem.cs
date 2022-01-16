using System.Collections.Generic;

namespace PegasusCms.Models.Page
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }

        public IEnumerable<MenuItem> SubMenuItems { get; set; }
    }
}
