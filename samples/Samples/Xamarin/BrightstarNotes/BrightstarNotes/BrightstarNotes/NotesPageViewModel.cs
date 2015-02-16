using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightstarNotes.Model;

namespace BrightstarNotes
{
    public class NotesPageViewModel
    {
        public string Title { get; set; }
        public ICollection<Note> Notes { get; set; }

        public bool HasItems { get { return Notes.Any(); } }
    }
}
