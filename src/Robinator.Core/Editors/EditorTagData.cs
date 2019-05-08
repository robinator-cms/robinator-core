using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Robinator.Core.Editors
{
    public class EditorTagData
    {
        /// <summary>
        /// Stores render data for editor fields.
        /// </summary>
        /// <param name="editorTag">The tag of the input</param>
        /// <param name="others">The tag of the additional scripts</param>
        public EditorTagData(TagBuilder editorTag, TagBuilder others = null)
        {
            EditorTag = editorTag;
            Others = others;
        }

        public TagBuilder EditorTag { get; }
        public TagBuilder Others { get; }
    }
}
