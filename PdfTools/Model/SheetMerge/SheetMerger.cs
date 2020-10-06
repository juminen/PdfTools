using JMI.General;
using JMI.General.Logging;
using JMI.General.Selections;
using System.Collections.Generic;
using System.Linq;

namespace PdfTools.Model.SheetMerge
{
    class SheetMerger : ObservableObject
    {
        #region constructors
        public SheetMerger()
        {
            Settings = new MergeSettings();
            Sheets = new SelectionCollection<Sheet>();
        }
        #endregion

        #region properties
        private readonly Logger logger = SingletonLogger.Instance;
        public SelectionCollection<Sheet> Sheets { get; }

        public MergeSettings Settings { get; private set; }

        //private bool createPdfFilesToFirtsSheetDirectory;
        //public bool CreatePdfFilesToFirtsSheetDirectory
        //{
        //    get { return createPdfFilesToFirtsSheetDirectory; }
        //    set { SetProperty(ref createPdfFilesToFirtsSheetDirectory, value); }
        //}

        //private string directoryPathForPdfs;
        //public string DirectoryPathForPdfs
        //{
        //    get { return directoryPathForPdfs; }
        //    set { SetProperty(ref directoryPathForPdfs, value); }
        //}

        //private bool createCombinationFile;
        //public bool CreateCombinationFile
        //{
        //    get { return createCombinationFile; }
        //    set { SetProperty(ref createCombinationFile, value); }
        //}

        //private string combinationFilePath;
        //public string CombinationFilePath
        //{
        //    get { return combinationFilePath; }
        //    set { SetProperty(ref combinationFilePath, value); }
        //}

        //private string sheetSeparator;
        //public string SheetSeparator
        //{
        //    get { return sheetSeparator; }
        //    set { SetProperty(ref sheetSeparator, value); }
        //}

        //private bool opendAfterMerging;
        //public bool OpendAfterMerging
        //{
        //    get { return opendAfterMerging; }
        //    set { SetProperty(ref opendAfterMerging, value); }
        //}
        #endregion

        #region methods
        //private void ReadSettings()
        //{
        //    CreatePdfFilesToFirtsSheetDirectory = Properties.Settings.Default.SaveToSameDirectoryAsFirstSheet;
        //    DirectoryPathForPdfs = Properties.Settings.Default.SaveDirectoryPath;
        //    CreateCombinationFile = Properties.Settings.Default.CreateCombinationFile;
        //    CombinationFilePath = Properties.Settings.Default.CombinationFilePath;
        //    SheetSeparator = Properties.Settings.Default.SheetSeparator;
        //    OpendAfterMerging = Properties.Settings.Default.OpenAfterMerge;
        //}

        //public void SaveSettings()
        //{
        //    Properties.Settings.Default.SaveToSameDirectoryAsFirstSheet = CreatePdfFilesToFirtsSheetDirectory;
        //    Properties.Settings.Default.SaveDirectoryPath = DirectoryPathForPdfs;
        //    Properties.Settings.Default.CreateCombinationFile = CreateCombinationFile;
        //    Properties.Settings.Default.CombinationFilePath = CombinationFilePath;
        //    Properties.Settings.Default.SheetSeparator = SheetSeparator;
        //    Properties.Settings.Default.OpenAfterMerge = OpendAfterMerging;
        //    Properties.Settings.Default.Save();
        //}

        public void AddSheet(string filepath)
        {
            if (!filepath.EndsWith(".pdf"))
            {
                string msg = $"File '{filepath}' discarded (not pdf).";
                logger.Log(LogFactory.CreateWarningMessage(msg));
                return;
            }

            foreach (Sheet item in Sheets.GetAllTargetItems())
            {
                if (item.FullPath.Equals(filepath))
                {
                    string msg = $"File '{filepath}' was already in the list.";
                    logger.Log(LogFactory.CreateWarningMessage(msg));
                    return;
                }
            }

            if (!CheckSheetSeparator())
            {
                return;
            }

            string fileName = System.IO.Path.GetFileName(filepath);

            if (!fileName.Contains(Settings.SheetSeparator))
            {
                string msg = $"File name '{fileName}' does not contain sheet separator '{Settings.SheetSeparator}', file discarded.";
                logger.Log(LogFactory.CreateWarningMessage(msg));
                return;
            }

            Sheet sheet = new Sheet(filepath, Settings.SheetSeparator);
            Sheets.AddItem(sheet);
        }

        public void MergeSheets()
        {
            if (!CheckSheetFileExistence())
            {
                string msg = $"Pdf merging stopped, check files.";
                logger.Log(LogFactory.CreateWarningMessage(msg));
                return;
            }

            if (!CheckInitialSettings())
            {
                string msg = $"Pdf merging stopped, check settings.";
                logger.Log(LogFactory.CreateWarningMessage(msg));
                return;
            }

            SheetSetBuilder setBuilder = new SheetSetBuilder();
            List<SheetSet> sets = setBuilder.CreateSheetSets(Sheets.GetAllTargetItems(), Settings).ToList();
            PdfMerger.MergePdfs(sets, Settings);

            if (Settings.OpendAfterMerging)
            {
                foreach (SheetSet set in sets)
                {
                    System.Diagnostics.Process.Start(set.FullPath);
                }

                if (Settings.CreateCombinationFile)
                {
                    if (!System.IO.File.Exists(Settings.CombinationFilePath))
                    {
                        string msg = $"Can not open file '{Settings.CombinationFilePath}', file does not exists.";
                        logger.Log(LogFactory.CreateWarningMessage(msg));
                        return;
                    }
                    System.Diagnostics.Process.Start(Settings.CombinationFilePath);
                }
                else
                {
                    foreach (SheetSet set in sets)
                    {
                        if (!System.IO.File.Exists(set.FullPath))
                        {
                            string msg = $"Can not open file '{set.FullPath}', file does not exists.";
                            logger.Log(LogFactory.CreateWarningMessage(msg));
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(set.FullPath);
                        }
                    }
                }
            }
        }

        private bool CheckSheetSeparator()
        {
            //Not empty or contain only spaces
            if (string.IsNullOrWhiteSpace(Settings.SheetSeparator))
            {
                string msg = $"Sheet separator can not be empty or contain only spaces.";
                logger.Log(LogFactory.CreateErrorMessage(msg));
                return false;
            }
            return true;
        }

        private bool CheckSheetFileExistence()
        {
            bool result = true;

            foreach (Sheet s in Sheets.GetAllTargetItems())
            {
                if (!s.Exists())
                {
                    result = false;
                    string msg = $"File '{s.FullPath}' does not exists.";
                    logger.Log(LogFactory.CreateErrorMessage(msg));
                }
            }
            return result;
        }

        private bool CheckInitialSettings()
        {
            //Saving pdf files to
            bool result1 = true;
            if (!Settings.CreatePdfFilesToFirstSheetDirectory)
            {
                if (string.IsNullOrWhiteSpace(Settings.DirectoryPathForPdfs))
                {
                    result1 = false;
                    string msg = $"Path for all files can not be empty.";
                    logger.Log(LogFactory.CreateErrorMessage(msg));
                }
                else if (!System.IO.Directory.Exists(Settings.DirectoryPathForPdfs))
                {
                    result1 = false;
                    string msg = $"Path '{Settings.DirectoryPathForPdfs}' for all files does not exists.";
                    logger.Log(LogFactory.CreateErrorMessage(msg));
                }
            }

            //Combination file
            bool result2 = true;
            if (Settings.CreateCombinationFile)
            {
                if (string.IsNullOrWhiteSpace(Settings.CombinationFilePath))
                {
                    result2 = false;
                    string msg = $"Path for combination file can not be empty.";
                    logger.Log(LogFactory.CreateErrorMessage(msg));
                }
                else if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Settings.CombinationFilePath)))
                {
                    result2 = false;
                    string msg = $"Directory '{System.IO.Path.GetDirectoryName(Settings.CombinationFilePath)}' for combination file does not exists.";
                    logger.Log(LogFactory.CreateErrorMessage(msg));
                }
            }

            return result1 & result2;
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
