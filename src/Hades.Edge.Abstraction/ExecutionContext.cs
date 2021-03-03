using System;
using System.Collections.Generic;

namespace Hades.Edge.Abstraction
{
    public class ExecutionContext
    {
        public IList<Type> Agents { get; } = new List<Type>();
    }
}
