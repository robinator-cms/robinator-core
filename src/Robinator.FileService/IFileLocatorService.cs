using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Robinator.FileService
{
    public interface IFileLocatorService
    {
        Directory GetRootDirectory();
        Task<Directory> GetRootDirectoryAsync();
        ICollection<File> GetFiles(Directory directory, FileFilter fileFilter = default);
        Task<ICollection<File>> GetFilesAsync(Directory directory, FileFilter fileFilter = default);
        ICollection<Directory> GetDirectories(Directory directory, DirectoryFilter directoryFilter = default);
        Task<ICollection<Directory>> GetDirectoriesAsync(Directory directory, DirectoryFilter directoryFilter = default);
        Uri GetPublicUri(File file);
        Task<Uri> GetPublicUriAsync(File file);
    }
}
