using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Robinator.FileService.UI.Default.Areas.RobinatorAdmin.Pages.FileUploader
{
    public class IndexModel : PageModel
    {
        private readonly IFileLocatorService fileLocatorService;
        private string path;

        [BindProperty(SupportsGet = true)]
        public string Path { get => path; set => path = string.IsNullOrEmpty(value) ? null : Uri.UnescapeDataString(value).Trim('/'); }
        public ICollection<IDirectory> Directories { get; private set; }
        public ICollection<IFile> Files { get; private set; }
        public bool NewFolder { get; private set; }

        public IndexModel(IFileLocatorService fileLocatorService)
        {
            this.fileLocatorService = fileLocatorService;
        }
        public async Task OnGetAsync()
        {
            IDirectory directory = await GetDirectory();
            Directories = await fileLocatorService.GetDirectoriesAsync(directory);
            Files = await fileLocatorService.GetFilesAsync(directory);
        }

        private async Task<IDirectory> GetDirectory()
        {
            IDirectory directory = null;
            if (string.IsNullOrEmpty(Path))
            {
                directory = await fileLocatorService.GetRootDirectoryAsync();
                Path = null;
            }
            else
            {
                directory = await fileLocatorService.CreateDirectoryFromPathAsync(Path);
            }
            if (!await fileLocatorService.ExistsAsync(directory))
            {
                directory = await fileLocatorService.GetRootDirectoryAsync();
                Path = null;
            }

            return directory;
        }

        public void OnGetNewDirectoryAsync()
        {
            NewFolder = true;
        }
        [BindProperty]
        public string NewDirectoryName { get; set; }
        public async Task OnPostNewDirectoryAsync()
        {
            if (string.IsNullOrEmpty(NewDirectoryName))
            {
                OnGetNewDirectoryAsync();
                ModelState.AddModelError("NewDirectoryName", "New directory name is required.");
                return;
            }
            await fileLocatorService.CreateDirectoryAsync(await GetDirectory(), NewDirectoryName);
            await OnGetAsync();
        }
    }
}
