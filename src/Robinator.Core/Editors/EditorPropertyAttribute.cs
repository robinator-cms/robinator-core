using System;

namespace Robinator.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class EditorPropertyAttribute : Attribute
    {
        public EditorPropertyAttribute(Type editor)
        {
            Editor = editor ?? throw new ArgumentNullException(nameof(editor));
            if (!editor.IsInterface)
            {
                throw new ArgumentException("The editor argument of EditorProperty must be an interface.");
            }
            if (typeof(IEditor) == editor)
            {
                throw new ArgumentException("The editor argument of EditorProperty must not be IEditor.");
            }
            if (!typeof(IEditor).IsAssignableFrom(editor))
            {
                throw new ArgumentException("The editor argument of EditorProperty must implements IEditor.");
            }
        }

        public Type Editor { get; }
    }
}
