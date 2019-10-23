using Robinator.Core;
using System.Collections.Generic;

namespace Robinator.Example.Components
{
    public interface IRateable : IContent
    {
        public ICollection<Rating> Stars { get; }
    }
}
