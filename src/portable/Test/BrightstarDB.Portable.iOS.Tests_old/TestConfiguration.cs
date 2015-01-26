using System;

namespace BrightstarDB.Portable.iOS.Tests
{
    public static class TestConfiguration
    {
        public static string StoreLocation
        {
            get { return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "BrightstarDB"); }
        }
    }
}