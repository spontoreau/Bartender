using System;

namespace Bartender.Tests.Context
{
    public static class ContextFactory
    {
        public static T Get<T>() where T : IValue
        {
            var value = Activator.CreateInstance<T>();
            value.Value = Guid.NewGuid().ToString();
            return value;
        }
    }
}