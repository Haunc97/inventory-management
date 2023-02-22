using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class InventoryTransation
    {
        public int InventoryTransationId { get; set; }

        public string PONumber { get; set; } = string.Empty;

        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public InventoryTransationType ActivityType { get; set; }

        [Required]
        public int QuantityAfter { get; set; }

        public double UnitPrice { get; set; }

        [Required]
        public DateTime TransationDate { get; set; }

        [Required]
        public string DoneBy { get; set; } = string.Empty;

        public Inventory? Inventory { get; set; }
    }
}
