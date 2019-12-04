using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Robinator.FileService.LocalProvider
{
    public class LocalFileLocatorOptions
    {
        private string rootPath;

        public string RootPath { get => rootPath; set => rootPath = Path.GetFullPath(value); }
        public string PublicPath { get; set; }
    }
}
