using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccountingSystem.Core.Attribute;

namespace AccountingSystem.Models.TestUi
{
    public class IndexViewModel
    {
        [DisplayName("Language")]
        [AutoCompleteUiHint("GetAutoComplete", "TestUi")]
        public int Name { get; set; }
        
    }
}
