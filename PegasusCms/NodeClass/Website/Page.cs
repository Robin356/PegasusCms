using Pegasus.Core.Data;
using Pegasus.Core.Data.CustomFields;
using Pegasus.Core.NodeClass.Attributes;
using Pegasus.Core.NodeClass.Models.System;

namespace PegasusCms.NodeClass.Website
{
    // Note: Properties must be virtual
    [Model("9096b169-f852-4a83-89fc-2300264f22a5")]
    public class Page : BaseModel
    {
        public Page(Node innerNode) : base(innerNode) { }

        [Field("0862ba47-7e11-4d34-b0ad-0a4db265efaf")]
        public virtual string PageTitle { get; set; }
        [Field("0862ba47-7e11-4d34-b0ad-0a4db265efaf", loadSingleField: true)]
        public virtual string PageTitleSingle { get; set; }
        [Field("80564a51-20ec-40f3-9051-8f8e0a0e4dbb", typeof(BooleanField), loadSingleField: true)]
        public virtual bool ShowInMenu { get; set; }
        [Field("364d661d-8f9e-400c-ab51-c9fb2db56cb8", loadSingleField: true)]
        public virtual string MenuTitle { get; set; }
    }
}
