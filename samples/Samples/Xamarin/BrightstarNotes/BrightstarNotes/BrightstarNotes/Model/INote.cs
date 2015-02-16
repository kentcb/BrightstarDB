using System.Collections.Generic;
using BrightstarDB.EntityFramework;

namespace BrightstarNotes.Model
{
    [Entity]
    public interface INote
    {
        string Id { get; }
        string Title { get; set; }
        string Body { get; set; }
        ICollection<ITag> Tags { get; set; } 
    }
}
