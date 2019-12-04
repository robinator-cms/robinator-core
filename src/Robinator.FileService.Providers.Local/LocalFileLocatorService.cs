using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robinator.FileService.LocalProvider
{
    public class LocalFileLocatorService : IFileLocatorService
    {
        private readonly LocalFileLocatorOptions options;

        public LocalFileLocatorService(IOptionsMonitor<LocalFileLocatorOptions> options)
        {
            this.options = options.CurrentValue;
        }

        public void CreateDirectory(IDirectory directory, string newDirectoryName)
        {
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(localDirectory.FullPath, newDirectoryName));
        }

        public Task CreateDirectoryAsync(IDirectory directory, string newDirectoryName)
        {
            CreateDirectory(directory, newDirectoryName);
            return Task.CompletedTask;
        }

        public IDirectory CreateDirectoryFromPath(string path)
        {
            return new LocalDirectory
            {
                Path = path,
                FullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(options.RootPath,path))
            };
        }

        public Task<IDirectory> CreateDirectoryFromPathAsync(string path)
        {
            return Task.FromResult(CreateDirectoryFromPath(path));
        }

        public IFile CreateFileFromPath(IDirectory directory, string path)
        {
            var relativePath = System.IO.Path.Combine(directory.Path, path);
            return new LocalFile {
                Path = relativePath,
                FullPath = System.IO.Path.GetFullPath(relativePath)
            };
        }

        public Task<IFile> CreateFileFromPathAsync(IDirectory directory, string path)
        {
            return Task.FromResult(CreateFileFromPath(directory, path));
        }

        public bool Exists(IDirectory directory)
        {
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            return System.IO.Directory.Exists(localDirectory.FullPath);
        }

        public bool Exists(IFile file)
        {
            if (!(file is LocalFile localFile))
            {
                throw new ArgumentException($"{nameof(file)} should be {nameof(LocalDirectory)}");
            }
            return System.IO.File.Exists(localFile.FullPath);
        }

        public Task<bool> ExistsAsync(IDirectory directory)
        {
            return Task.FromResult(Exists(directory));
        }

        public Task<bool> ExistsAsync(IFile file)
        {
            return Task.FromResult(Exists(file));
        }

        public ICollection<IDirectory> GetDirectories(IDirectory directory, DirectoryFilter directoryFilter = null)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            if (!localDirectory.FullPath.StartsWith(options.RootPath))
            {
                throw new ArgumentException($"File cannot be outside the root folder.");
            }
            return System.IO.Directory.GetDirectories(localDirectory.FullPath, $"{directoryFilter?.Prefix ?? ""}*")
                .Select(x => System.IO.Path.GetFileName(x))
                .Where(x => !x.StartsWith('.'))
                .Select(x => CreateDirectoryFromPath(x))
                .Cast<IDirectory>()
                .ToList();
        }

        public Task<ICollection<IDirectory>> GetDirectoriesAsync(IDirectory directory, DirectoryFilter directoryFilter = null)
        {
            return Task.FromResult(GetDirectories(directory, directoryFilter));
        }

        public ICollection<IFile> GetFiles(IDirectory directory, FileFilter fileFilter = null)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            if (!localDirectory.FullPath.StartsWith(options.RootPath))
            {
                throw new ArgumentException($"File cannot be outside the root folder.");
            }
            return System.IO.Directory.GetFiles(localDirectory.FullPath, $"{fileFilter?.Prefix ?? ""}*")
                .Select(x => System.IO.Path.GetFileName(x))
                .Where(x => !x.StartsWith('.'))
                .Select(x => CreateFileFromPath(localDirectory, x))
                .Cast<IFile>()
                .ToList();
        }

        public Task<ICollection<IFile>> GetFilesAsync(IDirectory directory, FileFilter fileFilter = null)
        {
            return Task.FromResult(GetFiles(directory, fileFilter));
        }

        public Uri GetPublicUri(IFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (!(file is LocalFile localFile))
            {
                throw new ArgumentException($"{nameof(file)} should be {nameof(LocalFile)}");
            }
            if (!localFile.FullPath.StartsWith(options.RootPath))
            {
                throw new ArgumentException($"File cannot be outside the root folder.");
            }
            var path = localFile.FullPath.Substring(options.RootPath.Length);
            var builder = new UriBuilder(options.PublicPath);
            builder.Path = System.IO.Path.Combine(builder.Path, path);
            return builder.Uri;
        }

        public Task<Uri> GetPublicUriAsync(IFile file)
        {
            return Task.FromResult(GetPublicUri(file));
        }

        public IDirectory GetRootDirectory()
        {
            return new LocalDirectory
            {
                Path = "/",
                FullPath = options.RootPath
            };
        }

        public Task<IDirectory> GetRootDirectoryAsync()
        {
            return Task.FromResult(GetRootDirectory());
        }
    }
}
