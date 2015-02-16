using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightstarNotes
{
    public interface IBrightstarConnector
    {
        string StoresDirectory { get; }
        string ConnectionString { get; }
        void Initialize();
    }
}
