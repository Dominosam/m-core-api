namespace Mover.API.Exceptions.Watch
{
    public class InvalidTimestampException : Exception
    {
        public InvalidTimestampException() : base("Invalid timestamp provided.")
        {
        }
    }
}
