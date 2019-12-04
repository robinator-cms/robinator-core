using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Robinator.FileService
{
    public interface IFileLocatorService
    {
        IDirectory GetRootDirectory();
        Task<IDirectory> GetRootDirectoryAsync();
        ICollection<IFile> GetFiles(IDirectory directory, FileFilter fileFilter = default);
        Task<ICollection<IFile>> GetFilesAsync(IDirectory directory, FileFilter fileFilter = default);
        ICollection<IDirectory> GetDirectories(IDirectory directory, DirectoryFilter directoryFilter = default);
        Task<ICollection<IDirectory>> GetDirectoriesAsync(IDirectory directory, DirectoryFilter directoryFilter = default);
        bool Exists(IDirectory directory);
        bool Exists(IFile file);
        Task<bool> ExistsAsync(IDirectory directory);
        Task<bool> ExistsAsync(IFile file);
        Uri GetPublicUri(IFile file);
        Task<Uri> GetPublicUriAsync(IFile file);
        IFile CreateFileFromPath(IDirectory directory, string path);
        IDirectory CreateDirectoryFromPath(string path);
        Task<IFile> CreateFileFromPathAsync(IDirectory directory, string path);
        Task<IDirectory> CreateDirectoryFromPathAsync(string path);
        void CreateDirectory(IDirectory directory, string newDirectoryName);
        Task CreateDirectoryAsync(IDirectory directory, string newDirectoryName);
    }
}
