using System;
using System.Collections.Generic;
using System.Linq;

namespace Robinator.Core
{
    internal static class TypeExtensions
    {
        public static IEnumerable<EditorProperty> GetAllEditorFields(this Type type)
        {
            return type.GetProperties()
                .Select(p => new EditorProperty
                {
                    Property = p,
                    Editor = p.GetCustomAttributes(true)
                        .Where(a => typeof(EditorPropertyAttribute).IsAssignableFrom(a.GetType()))
                        .Cast<EditorPropertyAttribute>()
                        .Select(a => a.Editor)
                        .FirstOrDefault()
                })
                .Where(x => x.Editor != null);
        }
    }
}
