using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Robinator.Core.Areas.RobinatorAdmin.Pages.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Robinator.Core.Areas.RobinatorAdmin.Pages
{
    public class EditorModel : PageModel
    {
        private readonly RobinatorOptions options;
        private readonly IServiceProvider serviceProvider;

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public IList<EditorFieldViewModel> EditorFields { get; set; }
        public string Title { get; private set; }

        public EditorModel(IOptionsMonitor<RobinatorOptions> options, IServiceProvider serviceProvider)
        {
            this.options = options.CurrentValue;
            this.serviceProvider = serviceProvider;
        }
        public async Task OnGetAsync()
        {
            var editorDefinition = options.DefaultEditPages.Find(x => x.Link == Type);
            if (editorDefinition == null)
            {
                throw new ArgumentOutOfRangeException($"No type is found with default edit link {Type}");
            }
            var dataType = editorDefinition.Type;
            object data = dataType.GetConstructor(new Type[0]).Invoke(new object[0]);
            if (Id != null)
            {
                var repository = CreateRepository(dataType);
                data = await repository.GetContentAsync(new Guid(Id));
            }
            EditorFields = dataType.GetAllEditorFields().Select(x =>
            {
                var editor = serviceProvider.GetService(x.Editor) as IEditor ?? serviceProvider.GetService(typeof(ITextEditor)) as IEditor;
                var editorData = editor.GetView(x.Property.Name, x.Property.GetValue(data));
                return new EditorFieldViewModel
                {
                    Html = editorData.EditorTag,
                    Others = editorData.Others,
                    Text = x.Property.GetCustomAttributes(true)
                        .Where(p => p.GetType() == typeof(DisplayNameAttribute))
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault()
                        ?.Name
                        ?? x.Property.Name,
                };
            }).ToList();
            Title = editorDefinition.Projection(data).Text ?? $"New {editorDefinition.Name}";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var editorDefinition = options.DefaultEditPages.Find(x => x.Link == Type);
            if (editorDefinition == null)
            {
                throw new ArgumentOutOfRangeException($"No type is found with default edit link {Type}");
            }
            var dataType = editorDefinition.Type;
            var repository = CreateRepository(dataType);
            var data = Id == null ? dataType.GetConstructor(new Type[0]).Invoke(new object[0]) : repository.GetContent(new Guid(Id));
            foreach (var x in dataType.GetAllEditorFields())
            {
                var editor = serviceProvider.GetService(x.Editor) as IEditor ?? serviceProvider.GetService(typeof(ITextEditor)) as IEditor;
                x.Property.SetValue(data, editor.GetValue(x.Property.Name, Request));
            }
            if (Id == null)
            {
                await repository.CreateContentAsync(data);
            }
            else
            {
                await repository.UpdateContentAsync(data);
            }
            return RedirectToPage("Editor", new
            {
                id = dataType.GetProperty("Id").GetValue(data),
                Type,
            });
        }

        private IContentRepository CreateRepository(Type dataType)
        {
            return serviceProvider.GetService(typeof(IContentRepository<>).MakeGenericType(dataType)) as IContentRepository;
        }
    }
}