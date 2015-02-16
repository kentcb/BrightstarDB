using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Foundation;
using UIKit;

namespace BrightstarNotes.iOS
{
    class BrightstarConnectorIOS : IBrightstarConnector
    {
        private readonly string _storesDirectory;
        private readonly string _connectionString;

        public BrightstarConnectorIOS()
        {
            var appSupport =
                NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory,
                    NSSearchPathDomain.User)[0];
            _storesDirectory = Path.Combine(appSupport.RelativePath, "brightstar");
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