using JMI.General;
using JMI.General.VM.IO.Picker;
using PdfTools.Model.SheetMerge;

namespace PdfTools.UI.ViewModel
{
    class SheetMergeSettingsViewModel : ObservableObject
    {
        #region constructors
        public SheetMergeSettingsViewModel(MergeSettings mergeSettings)
        {
            settings = mergeSettings;

            SeriesPathPicker = DefaultPickers.DirectoryPicker;
            SeriesPathPicker.SelectedPath = settings.DirectoryPathForPdfs;
            SeriesPathPicker.PropertyChanged += OnSeriesPathPickerPropertyChanged;

            CombinationFilePicker = DefaultPickers.SaveFilePicker;
            CombinationFilePicker.SelectedPath = settings.CombinationFilePath;
            CombinationFilePicker.FileFilters = JMI.General.IO.FileFilters.Pdf.Filter;
            CombinationFilePicker.PropertyChanged += OnCombinationFilePickerPropertyChanged;
        }
        #endregion constructors

        #region properties
        private readonly MergeSettings settings;

        private DirectoryPickerViewModel seriesPathPicker;
        public DirectoryPickerViewModel SeriesPathPicker
        {
            get { return seriesPathPicker; }
            private set { SetProperty(ref seriesPathPicker, value); }
        }

        public bool CurrentDirectorySelected
        {
            get { return settings.CreatePdfFilesToFirstSheetDirectory; }
            set
            {
                settings.CreatePdfFilesToFirstSheetDirectory = value;
                OnPropertyChanged(nameof(CurrentDirectorySelected));
                OnPropertyChanged(nameof(SeriesPathIsEnabled));
            }
        }

        public bool SeriesPathIsEnabled { get { return !CurrentDirectorySelected; } }

        private SaveFilePickerViewModel combinationFilePicker;
        public SaveFilePickerViewModel CombinationFilePicker
        {
            get { return combinationFilePicker; }
            private set { SetProperty(ref combinationFilePicker, value); }
        }

        public bool CreateCombinationSelected
        {
            get { return settings.CreateCombinationFile; }
            set
            {
                settings.CreateCombinationFile = value;
                OnPropertyChanged(nameof(CreateCombinationSelected));
                OnPropertyChanged(nameof(CombinationPathIsEnabled));
            }
        }

        public bool CombinationPathIsEnabled { get { return CreateCombinationSelected; } }

        public string SheetSeparator
        {
            get { return settings.SheetSeparator; }
            set
            {
                settings.SheetSeparator = value;
                OnPropertyChanged(nameof(SheetSeparator));
            }
        }

        public bool OpenAfterMerge
        {
            get { return settings.OpendAfterMerging; }
            set
            {
                settings.OpendAfterMerging = value;
                OnPropertyChanged(nameof(OpenAfterMerge));
            }
        }

        public string Author
        {
            get { return settings.Author; }
            set
            {
                settings.Author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public string Keywords
        {
            get { return settings.Keywords; }
            set
            {
                settings.Keywords = value;
                OnPropertyChanged(nameof(Keywords));
            }
        }

        public string Subject
        {
            get { return settings.Subject; }
            set
            {
                settings.Subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }
        #endregion properties

        #region commands
        private RelayCommand clearPdfInformationCommand;
        public RelayCommand ClearPdfInformationCommand
        {
            get
            {
                if (clearPdfInformationCommand == null)
                {
                    clearPdfInformationCommand =
                      new RelayCommand(
                          param => ClearPdfInformtion(),
                          param => true);
                }
                return clearPdfInformationCommand;
            }
        }
        #endregion commands

        #region methods
        private void ClearPdfInformtion()
        {
            Author = string.Empty;
            Keywords = string.Empty;
            Subject = string.Empty;
        }
        #endregion methods

        #region events
        private void OnSeriesPathPickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(DirectoryPickerViewModel.SelectedPath)))
            {
                settings.DirectoryPathForPdfs = SeriesPathPicker.SelectedPath;
            }
        }

        private void OnCombinationFilePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(SaveFilePickerViewModel.SelectedPath)))
            {
                settings.CombinationFilePath = CombinationFilePicker.SelectedPath;
            }
        }
        #endregion events
    }
}
