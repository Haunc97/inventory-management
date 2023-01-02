﻿using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetByNameAsync(string name);
        Task AddAsync(Inventory inventory);
    }
}