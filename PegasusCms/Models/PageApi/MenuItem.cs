using System;
using System.Collections.Generic;

namespace PegasusCms.Models.PageApi
{
    public class MenuItem
    {
        public string Title { get; set; }
        public Guid PageId { get; set; }
        public bool Active { get; set; }

        public IEnumerable<MenuItem> SubMenuItems { get; set; }
    }
}
