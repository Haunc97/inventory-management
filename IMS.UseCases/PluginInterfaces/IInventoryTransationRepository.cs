using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransationRepository
    {
        void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price);
    }
}