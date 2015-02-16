using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightstarDB.EntityFramework;

namespace BrightstarNotes.Model
{
    [Entity]
    public interface ITag
    {
        string Id { get; }
        string Title { get; }
    }
}
