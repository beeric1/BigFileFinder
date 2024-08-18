
namespace BigFileFinder
{
    public class InvalidArgumentsException(string message, Exception? innerException) : Exception(message, innerException)
    {

        public InvalidArgumentsException(string message) : this(message, null) { }
    }
}
