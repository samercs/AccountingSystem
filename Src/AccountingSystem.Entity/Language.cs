using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Entity
{
    public class Language
    {
        public int LanguageId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
