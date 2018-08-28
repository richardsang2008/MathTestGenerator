using System;

namespace ApplicationCore.Exceptions
{
    public class ValidationNotEmptyException  :Exception
    {
        public ValidationNotEmptyException(string message) : base(message)
        {
        }

        public ValidationNotEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}