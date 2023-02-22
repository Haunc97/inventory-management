﻿using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransationRepository
    {
        void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price);
        void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price);
        Task<IEnumerable<InventoryTransation>> GetInventoryTransationsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransationType? transactionType);
    }
}