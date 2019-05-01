namespace Robinator.Core
{
    public class TextEditorAttribute : EditorPropertyAttribute
    {
        public TextEditorAttribute() : base(typeof(ITextEditor))
        {
        }
    }
}
