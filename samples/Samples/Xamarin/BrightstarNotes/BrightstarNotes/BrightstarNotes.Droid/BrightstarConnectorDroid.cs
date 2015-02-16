using System;
using System.IO;
using Path = BrightstarDB.Portable.Compatibility.Path;

namespace BrightstarNotes.Droid
{
    public class BrightstarConnectorDroid: IBrightstarConnector
    {
        private readonly string _connectionString;
        private readonly string _storesDirectory;

        public BrightstarConnectorDroid()
        {
            _storesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "brightstar");
            _connectionString = "type=embedded;storeName=notes;storesDirectory=" + _storesDirectory;
        }
        
        public string StoresDirectory
        {
            get { return _storesDirectory; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public void Initialize()
        {
            if (!Directory.Exists(_storesDirectory))
            {
                Directory.CreateDirectory(_storesDirectory);
            }
        }
    }
}