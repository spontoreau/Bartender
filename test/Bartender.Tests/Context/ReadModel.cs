using System;

namespace Bartender.Test.Context
{
    public class ReadModel 
    { 
        public string Value { get; set; }

        public static ReadModel New() => new ReadModel { Value = Guid.NewGuid().ToString() };
    }
}