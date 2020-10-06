using JMI.General;

namespace PdfTools.Model.SheetMerge
{
    class MergeSettings : ObservableObject
    {
        #region constructors
        public MergeSettings()
        {
            ReadSettings();
        }
        #endregion

        #region properties
        private bool createPdfFilesToFirstSheetDirectory;
        public bool CreatePdfFilesToFirstSheetDirectory
        {
            get { return createPdfFilesToFirstSheetDirectory; }
            set { SetProperty(ref createPdfFilesToFirstSheetDirectory, value); }
        }

        private string directoryPathForPdfs;
        public string DirectoryPathForPdfs
        {
            get { return directoryPathForPdfs; }
            set { SetProperty(ref directoryPathForPdfs, value); }
        }

        private bool createCombinationFile;
        public bool CreateCombinationFile
        {
            get { return createCombinationFile; }
            set { SetProperty(ref createCombinationFile, value); }
        }

        private string combinationFilePath;
        public string CombinationFilePath
        {
            get { return combinationFilePath; }
            set { SetProperty(ref combinationFilePath, value); }
        }

        private string sheetSeparator;
        public string SheetSeparator
        {
            get { return sheetSeparator; }
            set { SetProperty(ref sheetSeparator, value); }
        }

        private bool opendAfterMerging;
        public bool OpendAfterMerging
        {
            get { return opendAfterMerging; }
            set { SetProperty(ref opendAfterMerging, value); }
        }

        private string author;
        public string Author
        {
            get { return author; }
            set { SetProperty(ref author, value); }
        }

        public string Creator
        {
            get { return "Pdf Tools Sheet Set Merge"; }            
        }

        private string keywords;
        public string Keywords
        {
            get { return keywords; }
            set { SetProperty(ref keywords, value); }
        }

        private string subject;
        public string Subject
        {
            get { return subject; }
            set { SetProperty(ref subject, value); }
        }
        #endregion

        #region methods
        private void ReadSettings()
        {
            CreatePdfFilesToFirstSheetDirectory = Properties.Settings.Default.SaveToSameDirectoryAsFirstSheet;
            DirectoryPathForPdfs = Properties.Settings.Default.SaveDirectoryPath;
            CreateCombinationFile = Properties.Settings.Default.CreateCombinationFile;
            CombinationFilePath = Properties.Settings.Default.CombinationFilePath;
            SheetSeparator = Properties.Settings.Default.SheetSeparator;
            OpendAfterMerging = Properties.Settings.Default.OpenAfterMerge;
            Author = Properties.Settings.Default.Author;
            Keywords = Properties.Settings.Default.Keywords;
            Subject = Properties.Settings.Default.Subject;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.SaveToSameDirectoryAsFirstSheet = CreatePdfFilesToFirstSheetDirectory;
            Properties.Settings.Default.SaveDirectoryPath = DirectoryPathForPdfs;
            Properties.Settings.Default.CreateCombinationFile = CreateCombinationFile;
            Properties.Settings.Default.CombinationFilePath = CombinationFilePath;
            Properties.Settings.Default.SheetSeparator = SheetSeparator;
            Properties.Settings.Default.OpenAfterMerge = OpendAfterMerging;
            Properties.Settings.Default.Author = Author;
            Properties.Settings.Default.Keywords = Keywords;
            Properties.Settings.Default.Subject = Subject;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
