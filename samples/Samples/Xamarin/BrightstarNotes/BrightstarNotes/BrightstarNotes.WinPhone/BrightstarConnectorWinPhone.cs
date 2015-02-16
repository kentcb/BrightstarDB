using System;
using System.Threading.Tasks;
using Windows.Storage;
using BrightstarDB.Portable.Compatibility;

namespace BrightstarNotes.WinPhone
{
    public class BrightstarConnectorWinPhone : IBrightstarConnector
    {
        private readonly string _connectionString;
        private readonly string _storesDirectory;

        public BrightstarConnectorWinPhone()
        {
            var appDataLocal = ApplicationData.Current.LocalFolder.Path;
            _storesDirectory = Path.Combine(appDataLocal, "brightstar");
            _connectionString = "type=embedded;storeName=notes;storesDirectory=" + _storesDirectory;
        }

        public string StoresDirectory
        {
            get { return _storesDirectory; }
        }

        public string ConnectionString
        {
            get { return _connectionString;}
        }

        public void Initialize()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        public async Task InitializeAsync()
        {
            try
            {
                await StorageFolder.GetFolderFromPathAsync(_storesDirectory);
                return;
            }
            catch (Exception)
            {
            }
            await ApplicationData.Current.LocalFolder.CreateFolderAsync("brightstar");
        }
    }
}
