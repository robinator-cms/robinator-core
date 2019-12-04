using Robinator.Core;

namespace Robinator.FileService
{
    public class FileEditorAttribute : EditorPropertyAttribute
    {
        public FileEditorAttribute() : base(typeof(IFileEditor))
        {
        }
    }
}
