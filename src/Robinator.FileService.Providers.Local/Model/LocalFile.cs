namespace Robinator.FileService.LocalProvider
{
    class LocalFile : IFile
    {
        private string path;

        public string Path { get => path; set => path = value.Trim('/', System.IO.Path.DirectorySeparatorChar); }
        public string FullPath { get; set; }
    }
}
