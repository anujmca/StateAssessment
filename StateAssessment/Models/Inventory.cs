using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateAssessment.Models
{
    [Table("Inventory")]
    public partial class Inventory
    {
        public Inventory()
        {
            InverseParentInventory = new HashSet<Inventory>();
            Questions = new HashSet<Question>();
        }

        [Key]
        public long InventoryId { get; set; }
        public string? SectionName { get; set; }
        public string? InventoryName { get; set; }
        public string? InventoryDescription { get; set; }
        public int? TimeRequiredInMinutes { get; set; }
        public long? ParentInventoryId { get; set; }

        public virtual Inventory? ParentInventory { get; set; }
        public virtual ICollection<Inventory> InverseParentInventory { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
