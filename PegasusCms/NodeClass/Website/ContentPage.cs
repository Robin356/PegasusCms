using Pegasus.Core.Data;
using Pegasus.Core.NodeClass.Attributes;

namespace PegasusCms.NodeClass.Website
{
    // Note: Properties must be virtual
    [Model("f9a97878-801a-4de8-9451-29c73f5e89a2")]
    public class ContentPage : Page
    {
        public ContentPage(Node innerNode) : base(innerNode) { }

        [Field("32116705-8fdf-4036-8a34-f97138b521a7")]
        public virtual string ContentTitle { get; set; }
        [Field("cea95b01-d952-4793-beb2-f3984c265840")]
        public virtual string ContentBody { get; set; }
    }
}
