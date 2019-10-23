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

        public ICollection<Directory> GetDirectories(Directory directory, DirectoryFilter directoryFilter = null)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            return System.IO.Directory.GetDirectories(localDirectory.Path, $"{directoryFilter?.Prefix ?? ""}*").Select(x => new LocalDirectory
            {
                Path = x
            }).Cast<Directory>().ToList();
        }

        public Task<ICollection<Directory>> GetDirectoriesAsync(Directory directory, DirectoryFilter directoryFilter = null)
        {
            return Task.FromResult(GetDirectories(directory, directoryFilter));
        }

        public ICollection<File> GetFiles(Directory directory, FileFilter fileFilter = null)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (!(directory is LocalDirectory localDirectory))
            {
                throw new ArgumentException($"{nameof(directory)} should be {nameof(LocalDirectory)}");
            }
            return System.IO.Directory.GetFiles(localDirectory.Path, $"{fileFilter?.Prefix ?? ""}*").Select(x => new LocalFile
            {
                Path = x
            }).Cast<File>().ToList();
        }

        public Task<ICollection<File>> GetFilesAsync(Directory directory, FileFilter fileFilter = null)
        {
            return Task.FromResult(GetFiles(directory, fileFilter));
        }

        public Uri GetPublicUri(File file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (!(file is LocalFile localFile))
            {
                throw new ArgumentException($"{nameof(file)} should be {nameof(LocalFile)}");
            }
            var path = System.IO.Path.GetFullPath(localFile.Path);
            if (!localFile.Path.StartsWith(options.RootPath))
            {
                throw new ArgumentException($"File cannot be outside the root folder.");
            }
            path = path.Substring(options.RootPath.Length);
            var builder = new UriBuilder(options.PublicPath);
            builder.Path = System.IO.Path.Combine(builder.Path, path);
            return builder.Uri;
        }

        public Task<Uri> GetPublicUriAsync(File file)
        {
            return Task.FromResult(GetPublicUri(file));
        }

        public Directory GetRootDirectory()
        {
            return new LocalDirectory
            {
                Path = options.RootPath
            };
        }

        public Task<Directory> GetRootDirectoryAsync()
        {
            return Task.FromResult(GetRootDirectory());
        }
    }
}
