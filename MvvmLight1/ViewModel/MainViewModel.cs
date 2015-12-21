using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight;
using MSCFB;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        public ObservableCollection<DirectoryEntry> ObservableCollection { get; set; }
        private string _welcomeTitle = string.Empty;
        private CompoundFile _compoundFile;
        private DirectoryEntry _directoryEntry;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        public DirectoryEntry DirectoryEntry
        {
            get { return _directoryEntry; }
            set { _directoryEntry = value; RaisePropertyChanged("DirectoryEntry"); }
        }

        public CompoundFile CompoundFile
        {
            get { return _compoundFile; }
            set
            {
                _compoundFile = value;
                RaisePropertyChanged("CompoundFile");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            using (var fs = File.OpenRead("empty.msg"))
            {
                CompoundFile = new CompoundFile(fs);
                DirectoryEntry = CompoundFile.DirectoryChain.RootEntry;
                ObservableCollection = new ObservableCollection<DirectoryEntry>();
                ObservableCollection.Add(DirectoryEntry);

            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}