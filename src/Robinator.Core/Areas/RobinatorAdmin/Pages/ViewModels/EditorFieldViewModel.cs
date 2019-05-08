using Microsoft.AspNetCore.Mvc.Rendering;

namespace Robinator.Core.Areas.RobinatorAdmin.Pages.ViewModels
{
    public class EditorFieldViewModel
    {
        public TagBuilder Others { get; set; }
        public TagBuilder Html { get; set; }
        public string Text { get; set; }
    }
}
