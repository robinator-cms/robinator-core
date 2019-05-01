using System;
using System.Reflection;

namespace Robinator.Core
{
    internal class EditorProperty
    {
        public PropertyInfo Property { get; set; }
        public Type Editor { get; set; }
    }
}
