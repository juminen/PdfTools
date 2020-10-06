using JMI.General.Logging;
using System.Collections.Generic;

namespace PdfTools.Model.SheetMerge
{
    class SheetSetBuilder
    {
        #region constructors
        public SheetSetBuilder()
        {
            sheetSets = new Dictionary<string, SheetSet>();
        }
        #endregion

        #region properties
        private readonly Logger logger = SingletonLogger.Instance;
        private readonly Dictionary<string, SheetSet> sheetSets;
        private MergeSettings mergeSettings;
        #endregion

        #region methods
        public IEnumerable<SheetSet> CreateSheetSets(IEnumerable<Sheet> sheets, MergeSettings settings)
        {
            mergeSettings = settings;
            foreach (Sheet s in sheets)
            {
                AddSheet(s);
            }
            return sheetSets.Values;
        }

        private void AddSheet(Sheet sheet)
        {
            if (sheetSets.ContainsKey(sheet.SheetSet))
            {
                SheetSet set = sheetSets[sheet.SheetSet];
                if (set.ContainsSheet(sheet))
                {
                    string msg = $"File '{sheet.FullPath}' was already in sheet set '{sheet.SheetSet}'.";
                    logger.Log(LogFactory.CreateWarningMessage(msg));
                }
                else
                {
                    AddSheetToSet(set, sheet);
                }
            }
            else
            {
                //Create new sheet shet
                string path = string.Empty;
                if (mergeSettings.CreatePdfFilesToFirstSheetDirectory)
                {
                    path = sheet.DirectoryPath;
                }
                else
                {
                    path = mergeSettings.DirectoryPathForPdfs;
                }
                SheetSet set = CreateSheetSet(path, sheet.SheetSet);
                AddSheetToSet(set, sheet);
            }
        }

        private SheetSet CreateSheetSet(string directory, string name)
        {
            SheetSet set = new SheetSet(System.IO.Path.Combine(directory, name) + ".pdf");
            sheetSets.Add(set.NameWithoutExtension, set);
            string msg = $"Created new sheet set '{set.NameWithoutExtension}'.";
            logger.Log(LogFactory.CreateNormalMessage(msg));
            return set;
        }

        private void AddSheetToSet(SheetSet set, Sheet sheet)
        {
            set.Sheets.AddItem(sheet);
            string msg = $"Sheet '{sheet.SheetIdentifier}' added to sheet set '{sheet.SheetSet}'.";
            logger.Log(LogFactory.CreateNormalMessage(msg));
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
