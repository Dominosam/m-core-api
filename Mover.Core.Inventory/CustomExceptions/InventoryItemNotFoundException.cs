
namespace Mover.Core.Inventory.CustomExceptions
{
    public class InventoryItemNotFoundException : Exception
    {
        public InventoryItemNotFoundException(string message) : base(message)
        {
        }
    }
}
