using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightstarDB;
using BrightstarDB.Portable.Compatibility;
using BrightstarNotes.Model;
using BrightstarNotes.Pages;
using Xamarin.Forms;

namespace BrightstarNotes
{
    public class App : Application
    {
        private string _connectionString;

        public App()
        {
            GetConnectionString();
            IList<Note> notes;
            using (var context = new MyEntityContext(_connectionString))
            {
                notes = context.Notes.Take(10).Cast<Note>().ToList();
            }
            var vm = new NotesPageViewModel {Title = "Hello world", Notes = notes};
            MainPage = new NotesPage(vm);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            GetConnectionString();
        }

        private void GetConnectionString()
        {
            var bsConnector = DependencyService.Get<IBrightstarConnector>();
            bsConnector.Initialize();
            _connectionString = bsConnector.ConnectionString;
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            BrightstarDB.Client.BrightstarService.Shutdown();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
