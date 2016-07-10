using System;

namespace Bartender.Test.Context
{
    public class Query : IQuery 
    { 
        public string Value { get; set; }

        public static Query New() => new Query { Value = Guid.NewGuid().ToString() };
    }
}