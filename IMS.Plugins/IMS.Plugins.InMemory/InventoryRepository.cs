using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;
        public InventoryRepository()
        {
            _inventories = new List<Inventory>
            {
                new Inventory {InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2},
                new Inventory {InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15},
                new Inventory {InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 8},
                new Inventory {InventoryId = 4, InventoryName = "Bike Pedels", Quantity = 20, Price = 1},
            };
        }

        public async Task<IEnumerable<Inventory>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return await Task.FromResult(_inventories);

            return _inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task AddAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var maxId = _inventories.Max(x => x.InventoryId);
            inventory.InventoryId = maxId + 1;

            _inventories.Add(inventory);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryId != inventory.InventoryId && x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var model = _inventories.SingleOrDefault(x => x.InventoryId == inventory.InventoryId);

            if (model != null)
            {
                model.InventoryName = inventory.InventoryName;
                model.Quantity = inventory.Quantity;
                model.Price = inventory.Price;
            }

            return Task.CompletedTask;
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var inv = _inventories.First(x => x.InventoryId == inventoryId);
            var newInv = new Inventory
            {
                InventoryId = inv.InventoryId,
                InventoryName = inv.InventoryName,
                Quantity = inv.Quantity,
                Price = inv.Price
            };
            return await Task.FromResult(newInv);
        }
    }
}