using System;

namespace Utils
{
    public class NoComponentException : Exception
    {
        public NoComponentException(string message = "") : base(message) {}
    }
}