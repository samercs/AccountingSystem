using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingSystem.Entity
{
    public class StockCode: EntityBase
    {
        public int StockCodeId { get; set; }
        [Required, StringLength(50)]
        public string CodeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
