using System;

namespace Bartender
{
    public class DispatcherException : Exception
    {
        public DispatcherException(string message)
            : base(message)
        {
            
        }
    }
}