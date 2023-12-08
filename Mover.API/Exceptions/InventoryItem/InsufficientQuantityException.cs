namespace Mover.API.Exceptions.InventoryItem
{
    public class InsufficientQuantityException : Exception
    {
        public InsufficientQuantityException(string message) : base(message)
        {
        }
    }
}
