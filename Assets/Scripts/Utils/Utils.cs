using System;

namespace Utils
{
    public class NotGetException : Exception
    {
        public NotGetException(string message = "") : base(message) {}
    }
}