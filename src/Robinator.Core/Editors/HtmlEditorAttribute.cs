namespace Robinator.Core
{
    public class HtmlEditorAttribute : EditorPropertyAttribute
    {
        public HtmlEditorAttribute() : base(typeof(IHtmlEditor))
        {
        }
    }
}
